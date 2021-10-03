using System;
using System.Collections;
using System.Collections.Generic;
using Deblue.Battle;
using Deblue.ColliderFinders;
using Deblue.Extensions;
using Deblue.InteractiveObjects;
using Deblue.ObservingSystem;
using Deblue.SceneManagement;
using Deblue.Stats;
using LD49.Stats;
using UnityEngine;
using Zenject;

namespace LD49.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(HeroInstaller))]
    public class HeroController : MonoBehaviour, IDmgReceiver, IItemTaker
    {
        [SerializeField] private SceneChanger _sceneChanger;

        private LimitedStatsStorage<HeroStatId> _stats;

        private HeroView _view;
        private HeroModel _model;

        private Coroutine _pushingAwayFromWallCoroutine;
        private Coroutine _invincibleCoroutine;
        private Rigidbody2D _rigidbody;

        private readonly List<IObserver> _observers = new List<IObserver>(5);

        private IGroundChecker _floorChecker;
        private IGroundChecker _wallsChecker;

        public AttackerType AttackerType => HeroModel.AttackerType;

        public bool IsInvincible => false;

        public InteractionItem NearInteractionItem { get; set; }

        [Inject]
        public void Construct(LimitedStatsStorage<HeroStatId> stats)
        {
            _stats = stats;
            var hp = stats.GetStatProperty(HeroStatId.Hp);
            hp.ReachedLowerLimit.Subscribe(Die, _observers);
        }

        public void Init(HeroBindings bind)
        {
            _model = bind.Model;
            _model.OnGrounded.Subscribe(ResetJumps, _observers);
            _view = bind.View;

            _floorChecker = bind.FloorChecker;
            _wallsChecker = bind.WallsChecker;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            Teleport(_rigidbody.position);
        }

        private void OnDisable()
        {
            _observers.ClearObservers();
        }

        private void FixedUpdate()
        {
            CheckGround();
            UpdateRigidbody();
            UpdateView();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _floorChecker?.DrawGizmos();
            _wallsChecker?.DrawGizmos();
        }

#endif
        
        public void SubscribeMovementOnInput(Action<Action<Vector2Int>> subscription)
        {
            subscription.Invoke(SetInputDirection);
        }

        public void SubscribeJumpOnInput(Action<Action> subscription)
        {
            subscription.Invoke(Jump);
        }

        public void SubscribeInteractionOnInput(Action<Action> subscription)
        {
            subscription.Invoke(Interact);

            void Interact()
            {
                if (NearInteractionItem == null)
                    return;

                if (NearInteractionItem.CanHighlight)
                    NearInteractionItem.Interact();
            }
        }

        public void ApplyDamage(Damage dmg)
        {
            if (_model.IsInvincible)
                return;

            _view.ApplyDmg();
            _stats.ChangeAmount(HeroStatId.Hp, -dmg.Value);

            _invincibleCoroutine?.Do(StopCoroutine);
            _invincibleCoroutine = StartCoroutine(Invincible());

            StartCoroutine(GettingDmg());
        }

        public void Teleport(Vector2 position)
        {
            StartCoroutine(Teleporting(position));
        }

        private IEnumerator Teleporting(Vector2 position)
        {
            _pushingAwayFromWallCoroutine?.Do(StopCoroutine);
            _model.IsInvincible = true;
            _model.IsMoveLock = true;

            float gravityScale = _rigidbody.gravityScale;
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = Vector2.zero;
            
            _view.Glitch();
            yield return new WaitForSecondsRealtime(0.5f);
            
            transform.position = position;
            
            _view.Glitch();
            yield return new WaitForSecondsRealtime(0.5f);

            _rigidbody.gravityScale = gravityScale;
            _model.IsInvincible = false;
            _model.IsMoveLock = false;
        }
        

        private void CheckGround()
        {
            _model.IsGrounded = _floorChecker.IsColliderFound;

            _model.IsWallNear = _wallsChecker.IsColliderFound;
            _model.WallNormal = _wallsChecker.GroundNormal;
        }

        private void UpdateRigidbody()
        {
            _model.Position = _rigidbody.position;
            _model.Velocity = _rigidbody.velocity;
            MoveHorizontal(_model.IsMoveLock ? 0 : _model.InputDirection.x);
            LimitFallSpeed(_model.MaxFallSpeed);
        }

        private void LimitFallSpeed(float value)
        {
            if (_rigidbody.velocity.y < -value)
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -value);
        }

        private void UpdateView()
        {
            _view.VerticalMovement = _model.Velocity.y.GetClearDirection(0.2f);
            _view.InputDirection = _model.InputDirection;
            _view.IsMoveHorizontally = _model.HorizontalMoveDirection != 0;
            _view.IsLeaningOnWall = _model.IsLeaningOnWall;
            _view.Tern(_model.InputDirection.x);
        }

        private void Jump()
        {
            if(_model.IsMoveLock)
                return;
                
            if (_model.IsCanJumpFromWall)
                JumpFromWall();
            else if (_model.IsCanJumpFromGround)
                JumpFromGround();
        }

        private void JumpFromGround()
        {
            _model.JumpsMade++;
            SetVerticalVelocity(_model.JumpForce);
            
            _view.Jump();
        }

        private void JumpFromWall()
        {
            _model.JumpsMade = 1;
            _pushingAwayFromWallCoroutine?.Do(StopCoroutine);
            _pushingAwayFromWallCoroutine = StartCoroutine(PushingAwayFromWall());
            SetVerticalVelocity(_model.JumpForce);
            
            _view.Jump();
        }

        private void SetVerticalVelocity(float force)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, force);
        }

        private void SetVelocity(Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        private IEnumerator PushingAwayFromWall()
        {
            _model.IsMoveLock = true;

            int direction = _model.WallNormal.x.GetClearDirection();
            float timer = _model.JumpFromWallDuration;
            while (timer > 0f)
            {
                timer -= Time.fixedDeltaTime;
                MoveHorizontal(direction);
                yield return new WaitForFixedUpdate();
            }

            _model.IsMoveLock = false;
            _pushingAwayFromWallCoroutine = null;

            while (_model.InputDirection.x == 0 && !_model.IsGrounded && !_model.IsWallNear)
            {
                MoveHorizontal(direction);
                yield return new WaitForFixedUpdate();
            }
        }

        private void SetInputDirection(Vector2Int inputDirection)
        {
            _model.InputDirection = inputDirection;
        }

        private void MoveHorizontal(int direction)
        {
            var speed = _model.MoveSpeed;
            _rigidbody.MoveHorizontal(direction, speed);
            _model.HorizontalMoveDirection = direction;
        }

        private void ResetJumps(PlayerOnTheGround context)
        {
            _model.JumpsMade = 0;
        }

        private IEnumerator Invincible()
        {
            _model.IsInvincible = true;

            yield return new WaitForSecondsRealtime(0.5f);

            _model.IsInvincible = false;
            _invincibleCoroutine = null;
        }

        private void Die(PropertyReachedLowerLimit context)
        {
            StopAllCoroutines();
            
            _model.IsInvincible = true;
            StartCoroutine(Dying());
        }

        private IEnumerator Dying()
        {
            _view.Dead();
            yield return GettingDmg();
            _sceneChanger.LoadScene();
        }

        private IEnumerator GettingDmg()
        {
            yield return Teleporting(transform.position);
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}