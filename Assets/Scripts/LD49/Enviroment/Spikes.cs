using Deblue.Battle;
using UnityEngine;

namespace LD49.Enviroment
{
    public class Spikes : MonoBehaviour, IAttacker
    {
        [SerializeField] private AttackTrigger _attackTrigger;

        public AttackerType AttackerType => AttackerType.Enviroment;

        private void Awake()
        {
            _attackTrigger.Init(this);
        }

        public Damage GetDamage()
        {
            return new Damage(1, 0);
        }
    }
}
