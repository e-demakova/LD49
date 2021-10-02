using System.Collections.Generic;
using Deblue.ObservingSystem;
using Deblue.Story.DialogSystem;
using UnityEngine;

namespace Deblue.Story.Characters
{
    public readonly struct DialogTrigger
    {
        public readonly Character Character;
        public readonly bool IsEnter;

        public DialogTrigger(Character character, bool isEnter)
        {
            Character = character;
            IsEnter = isEnter;
        }
    }

    public readonly struct PlayerGaveItem
    {
        public readonly string CharacterId;
        public readonly string ItemId;

        public PlayerGaveItem(string characterId, string itemId)
        {
            CharacterId = characterId;
            ItemId = itemId;
        }
    }

    public interface IDialogStarter
    {
    }

    [RequireComponent(typeof(Collider2D))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private DialogSO _defaultDialog;
        [SerializeField] private GameObject _dialogCanBeStartedView;

        private readonly Handler<DialogTrigger> _playerTalk = new Handler<DialogTrigger>();
        private readonly Queue<DialogSO> _uniqDialogs = new Queue<DialogSO>(2);
        private bool IsHaveUniqDialogues => _uniqDialogs.Count > 0;
        public IReadOnlyHandler<DialogTrigger> PlayerTalk => _playerTalk;
        public string Id => _id;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDialogStarter>(out var dialogStarter))
            {
                _playerTalk.Raise(new DialogTrigger(this, true));
                _dialogCanBeStartedView?.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IDialogStarter>(out var dialogStarter))
            {
                _playerTalk.Raise(new DialogTrigger(this, false));
                _dialogCanBeStartedView?.SetActive(false);
            }
        }

        public DialogSO GetDialog()
        {
            return IsHaveUniqDialogues ? _uniqDialogs.Dequeue() : _defaultDialog;
        }

        public void GiveDialogs(DialogSO[] dialog)
        {
            for (int i = 0; i < dialog.Length; i++)
            {
                _uniqDialogs.Enqueue(dialog[i]);
            }
        }
    }
}