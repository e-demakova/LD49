using System;
using System.Collections.Generic;
using UnityEngine;
using Deblue.ObservingSystem;
using Deblue.SceneManagement;
using Deblue.Story.Characters;
using Object = UnityEngine.Object;

namespace Deblue.Story.DialogSystem
{
    [Serializable]
    public class Dialogs
    {
        public string CharacterId;
        public DialogSO[] DialogsSO;
    }

    public class DialogRequester
    {
        private readonly Dictionary<string, Dialogs> _dialogues = new Dictionary<string, Dialogs>();

        private readonly DialogSwitcher _switcher;
        private Character _nearToPlayerCharacter;
        private Character[] _charactersInScene = Array.Empty<Character>();
        private readonly List<IObserver> _observCharacters = new List<IObserver>(5);
        private readonly List<IObserver> _observers = new List<IObserver>(5);

        public DialogRequester(DialogSwitcher switcher, SceneLoader loader)
        {
            _switcher = switcher;
            loader.SceneLoadingStarted.Subscribe(obj => _observCharacters.ClearObservers(), _observers);
            loader.SceneLoaded.Subscribe(x => SetNewCharactersInScene(Object.FindObjectsOfType<Character>()), _observers);
        }

        public void SubscribeDialogRequestOnInput(Action<Action> subscription)
        {
            subscription.Invoke(RequestDialog);
        }

        public void DeInit()
        {
            _observCharacters.ClearObservers();
            _observers.ClearObservers();
        }

        public void UpdateDialogues(Dialogs[] dialogues)
        {
            SetDialogs(dialogues);
            GiveDialoguesToCharacters();
        }

        public void SetNewCharactersInScene(Character[] characters)
        {
            _charactersInScene = characters;
            SubscribeToCharacters();
            GiveDialoguesToCharacters();
        }

        private void SetDialogs(Dialogs[] dialogues)
        {
            _dialogues.Clear();

            for (int i = 0; i < dialogues.Length; i++)
            {
                _dialogues.Add(dialogues[i].CharacterId, dialogues[i]);
            }
        }

        private void SubscribeToCharacters()
        {
            for (int i = 0; i < _charactersInScene.Length; i++)
            {
                _charactersInScene[i].PlayerTalk.Subscribe(UpdateNearToPlayerCharacter, _observCharacters);
            }

            void UpdateNearToPlayerCharacter(DialogTrigger context)
            {
                _nearToPlayerCharacter = context.IsEnter ? context.Character : null;
            }
        }

        private void GiveDialoguesToCharacters()
        {
            for (int i = 0; i < _charactersInScene.Length; i++)
            {
                if (_dialogues.TryGetValue(_charactersInScene[i].Id, out var dialog))
                    _charactersInScene[i].GiveDialogs(dialog.DialogsSO);
            }
        }

        private void RequestDialog()
        {
            if (_nearToPlayerCharacter == null)
                return;

            _switcher.StartDialog(_nearToPlayerCharacter.GetDialog(), _nearToPlayerCharacter.Id);
        }
    }
}