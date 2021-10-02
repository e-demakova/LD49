using UnityEngine;
using Zenject;

namespace Deblue.Story.DialogSystem
{
    public class NextReplicaButton : MonoBehaviour
    {
        private DialogSwitcher _dialogSwitcher;

        [Inject]
        public void Construct(DialogSwitcher dialogSwitcher)
        {
            _dialogSwitcher = dialogSwitcher;
        }
    
        public void SwitchReplica()
        {
            _dialogSwitcher.SwitchReplicaOnButton();
        }
    }
}
