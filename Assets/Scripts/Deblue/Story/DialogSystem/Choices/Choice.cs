using UnityEngine;

namespace Deblue.Story.DialogSystem.Choices
{
    [System.Serializable]
    public class Choice
    {
        [System.NonSerialized] public bool IsAvailable;

        [SerializeField] private DialogSO _nextDialogue;
        [SerializeField] private string _textId;
        [SerializeField] private string _blockedTextId;
        [SerializeField] private string _itemId;

        public string TextId => _textId;
        public string ItemID => _itemId;
        public string BlockedTextId => _blockedTextId;
        public DialogSO NextDialogue => _nextDialogue;
    }
}