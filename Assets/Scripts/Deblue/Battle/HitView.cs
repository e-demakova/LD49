using System;
using System.Collections;
using Deblue.ObservingSystem;
using Deblue.Pools;
using UnityEngine;

namespace Deblue.Battle
{
    public class HitView : MonoBehaviour, IPoolItem
    {
        private readonly int _hitTrigger = Animator.StringToHash("Hit");

        private readonly Handler<PoolItemLifeEnd> _lifeEnded = new Handler<PoolItemLifeEnd>();
        private Animator _animator;
        private Coroutine _showingHitCoroutine;

        public IReadOnlyHandler<PoolItemLifeEnd> LifeEnded => _lifeEnded;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ShowHit()
        {
            _animator.SetTrigger(_hitTrigger);

            if (_showingHitCoroutine != null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("You are trying to use HitView which is currently in use.");
#endif
                return;
            }

            _showingHitCoroutine = StartCoroutine(ShowingHit());
        }

        private IEnumerator ShowingHit()
        {
            yield return null;

            var clipLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSecondsRealtime(clipLength);

            _lifeEnded.Raise(new PoolItemLifeEnd(this));
            _showingHitCoroutine = null;
        }

        public void DeInit()
        {
        }
    }
}