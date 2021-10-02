using System;
using Deblue.Story.DialogSystem.Choices;
using UnityEngine;

namespace Deblue.Story.DialogSystem
{
    [CreateAssetMenu(fileName = "New_Dialog", menuName = "Dialogs/Dialog")]
    public class DialogSO : ScriptableObject
    {
        public Choice[] Choices;

        [SerializeField] private string _choiceTextId;
        [SerializeField] private Replica[] _replicas;
        [SerializeField] private DialogStartType _startType;

        [NonSerialized] private int _elementIndex = -1;

        public DialogStartType StartType => _startType;
        public bool IsHaveChoices => Choices?.Length > 0;
        public string ChoiceTextId => _choiceTextId;
        private bool ElementsEnded => _elementIndex >= _replicas.Length - 1;

        public void Init()
        {
#if UNITY_EDITOR
            _elementIndex = -1;
#endif
        }

        public bool TrySwitchToNextReplica(out Replica replica)
        {
            replica = null;
            if (ElementsEnded)
            {
                return false;
            }

            _elementIndex++;
            replica = _replicas[_elementIndex];
            return true;
        }
    }

    public enum DialogStartType
    {
        Forcibly,
        Voluntarily
    }
}