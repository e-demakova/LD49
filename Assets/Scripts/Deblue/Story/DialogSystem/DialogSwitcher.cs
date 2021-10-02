using System;
using Deblue.Models;
using Deblue.ObservingSystem;
using Deblue.Story.Characters;
using Deblue.Story.DialogSystem.Choices;

namespace Deblue.Story.DialogSystem
{
    public class DialogSwitcher : IModel
    {
        public IDialogSwitcherEvents Events => _events;

        private readonly DialogSwitcherEvents _events = new DialogSwitcherEvents();
        
        private DialogSO _currentDialog;
        private IChoiceReciver _choiceReceiver;
        private string _characterTalkingWithPlayerId;

        public void StartDialog(DialogSO dialog, string characterId)
        {
            _currentDialog = dialog;
            _events.DialogStartedHandler.Raise(new DialogStarted(dialog, characterId));
            _characterTalkingWithPlayerId = characterId;
            _currentDialog.Init();
            SwitchReplica();
        }

        private void CloseCurrentDialog()
        {
            _events.DialogEndedHandler.Raise(new DialogEnded(_currentDialog, _characterTalkingWithPlayerId));
            _currentDialog = null;
        }

        public void SubscribeSwitchingReplicaOnInput(Action<Action> subscription)
        {
            subscription.Invoke(SwitchReplica);
        }

        public void SwitchReplicaOnButton()
        {
            SwitchReplica();
        }

        private void SwitchReplica()
        {
            if (_currentDialog.TrySwitchToNextReplica(out var replica))
                _events.ReplicaSwitchedHandler.Raise(new ReplicaSwitched(replica));
            else if (_currentDialog.IsHaveChoices)
                SetChoices();
            else
                CloseCurrentDialog();
        }

        private void SetChoices()
        {
            var choices = _currentDialog.Choices;
            for (int i = 0; i < choices.Length; i++)
            {
                choices[i].IsAvailable = _choiceReceiver.CheckChoiceAvalible(choices[i]);
            }

            _events.DialogGaveChoiceHandler.Raise(new DialogGaveChoice(choices, _currentDialog.ChoiceTextId));
        }

        public void OnChoiceMade(Choice choice)
        {
            _events.DialogChoiceMadeHandler.Raise(new DialogChoiceMade(choice));
            string requiredItem = choice.ItemID;
            
            if (string.IsNullOrEmpty(requiredItem)) 
                _events.PlayerGaveItemHandler.Raise(new PlayerGaveItem(_characterTalkingWithPlayerId, requiredItem));

            if (choice.NextDialogue != null)
                StartDialog(choice.NextDialogue, _characterTalkingWithPlayerId);
            else
                CloseCurrentDialog();
        }
    }

    public interface IDialogSwitcherEvents
    {
        IReadOnlyHandler<DialogStarted> DialogStarted { get; }
        IReadOnlyHandler<DialogEnded> DialogEnded { get; }
        IReadOnlyHandler<ReplicaSwitched> ReplicaSwitched { get; }
        IReadOnlyHandler<DialogGaveChoice> DialogGaveChoice { get; }
        IReadOnlyHandler<DialogChoiceMade> DialogChoiceMade { get; }
        IReadOnlyHandler<PlayerGaveItem> PlayerGaveItem { get; }
    }

    public class DialogSwitcherEvents : IDialogSwitcherEvents
    {
        public readonly Handler<DialogStarted> DialogStartedHandler = new Handler<DialogStarted>();
        public readonly Handler<DialogEnded> DialogEndedHandler = new Handler<DialogEnded>();
        public readonly Handler<ReplicaSwitched> ReplicaSwitchedHandler = new Handler<ReplicaSwitched>();
        public readonly Handler<DialogGaveChoice> DialogGaveChoiceHandler = new Handler<DialogGaveChoice>();
        public readonly Handler<PlayerGaveItem> PlayerGaveItemHandler = new Handler<PlayerGaveItem>();
        public readonly Handler<DialogChoiceMade> DialogChoiceMadeHandler = new Handler<DialogChoiceMade>();

        public IReadOnlyHandler<DialogStarted> DialogStarted => DialogStartedHandler;
        public IReadOnlyHandler<DialogEnded> DialogEnded => DialogEndedHandler;
        public IReadOnlyHandler<ReplicaSwitched> ReplicaSwitched => ReplicaSwitchedHandler;
        public IReadOnlyHandler<DialogGaveChoice> DialogGaveChoice => DialogGaveChoiceHandler;
        public IReadOnlyHandler<PlayerGaveItem> PlayerGaveItem => PlayerGaveItemHandler;
        public IReadOnlyHandler<DialogChoiceMade> DialogChoiceMade => DialogChoiceMadeHandler;
    }
}