using Deblue.Data;
using Deblue.Extensions;
using Deblue.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Deblue.Battle
{
    public enum AttackerType
    {
        Player,
        Enemy,
        Enviroment
    }

    public interface IAttacker
    {
        AttackerType AttackerType { get; }
        Damage GetDamage();
    }

    public interface IDmgReceiver
    {
        AttackerType AttackerType { get; }
        bool IsInvincible { get; }
        void ApplyDamage(Damage dmg);
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class AttackTrigger : MonoBehaviour
    {
        private readonly int _hitTrigger = Animator.StringToHash("Hit");

        [SerializeField] private AssetReference _hitViewAssetRef;

        private bool _isHaveHitsView;
        private Pool<HitView> _hitViewsFactory;
        private IAttacker _attacker;

        [Inject]
        private void Construct(LoadService loader)
        {
            _isHaveHitsView = !string.IsNullOrEmpty(_hitViewAssetRef.AssetGUID);

            if (_isHaveHitsView)
                _hitViewsFactory = new Pool<HitView>(_hitViewAssetRef, loader, 1);
        }

        public void Init(IAttacker owner)
        {
            _attacker = owner;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDmgReceiver>(out var dmgReceiver))
            {
                var isSameType = _attacker.AttackerType == dmgReceiver.AttackerType;
                var isEnemyHitEnviroment = _attacker.AttackerType == AttackerType.Enemy && dmgReceiver.AttackerType == AttackerType.Enviroment;

                if (!isSameType && !isEnemyHitEnviroment && !dmgReceiver.IsInvincible)
                {
                    var dmgReceiverPosition = other.transform.position;
                    
                    var dmg = _attacker.GetDamage();
                    dmg.TossAwayDirection = (dmgReceiverPosition - transform.position).x.GetClearDirection();
                    dmgReceiver.ApplyDamage(dmg);

                    ShowHit(dmgReceiverPosition);
                }
            }
        }

        private void ShowHit(Vector3 hitPosition)
        {
            if (!_isHaveHitsView)
                return;

            var hit = _hitViewsFactory.GetItem();
            hit.transform.position = hitPosition;
            hit.ShowHit();
        }
    }

    public struct Damage
    {
        public readonly int Value;
        public readonly float TossAwayForce;
        public float TossAwayDirection;

        public Damage(int dmg, float tossAwayForce)
        {
            Value = dmg;
            TossAwayForce = tossAwayForce;
            TossAwayDirection = 0;
        }
    }
}