using Deblue.Extensions;
using UnityEngine;

namespace LD49.Hero
{
    public class HeroView
    {
        private readonly Animator _hero;
        private readonly AudioSource _deadSound;
        private readonly AudioSource _jumpSound;
        private readonly AudioSource _dmgSound;

        private readonly int _isWallNear = Animator.StringToHash("IsWallNear");

        private readonly int _isMoveHorizontally = Animator.StringToHash("IsMoveHorizontally");
        private readonly int _verticalMovement = Animator.StringToHash("VerticalMovement");

        private readonly int _verticalInput = Animator.StringToHash("VerticalInput");
        private readonly int _horizontalInput = Animator.StringToHash("HorizontalInput");

        private readonly int _glitchTrigger = Animator.StringToHash("Glitch");

        public Quaternion Tern(float direction)
        {
            return _hero.transform.Tern(direction);
        }

        public Vector2Int InputDirection
        {
            set
            {
                _hero.SetInteger(_verticalInput, value.y);
                _hero.SetInteger(_horizontalInput, value.x);
            }
        }

        public int VerticalMovement
        {
            set => _hero.SetInteger(_verticalMovement, value);
        }

        public bool IsMoveHorizontally
        {
            set => _hero.SetBool(_isMoveHorizontally, value);
        }

        public bool IsLeaningOnWall
        {
            set => _hero.SetBool(_isWallNear, value);
        }

        public HeroView(Animator hero, AudioSource deadSound, AudioSource jumpSound, AudioSource dmgSound)
        {
            _hero = hero;
            _deadSound = deadSound;
            _jumpSound = jumpSound;
            _dmgSound = dmgSound;
        }

        public void ApplyDmg()
        {
            _hero.GetComponent<SpriteRenderer>().color = Color.red;
            _dmgSound.Play();
        }

        public void Glitch()
        {
            _hero.SetTrigger(_glitchTrigger);
        }

        public void Dead()
        {
            _hero.GetComponent<SpriteRenderer>().color = Color.red;
            _deadSound.Play();
        }

        public void Jump()
        {
            _jumpSound.Play();
        }
    }
}