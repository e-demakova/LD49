using Deblue.Extensions;
using UnityEngine;

namespace LD49
{
    public class HeroView
    {
        private readonly Animator _hero;
        
        private readonly int _isWallNear = Animator.StringToHash("IsWallNear");

        private readonly int _isMoveHorizontally = Animator.StringToHash("IsMoveHorizontally");
        private readonly int _verticalMovement = Animator.StringToHash("VerticalMovement");

        private readonly int _verticalInput = Animator.StringToHash("VerticalInput");
        private readonly int _horizontalInput = Animator.StringToHash("HorizontalInput");
        
        private readonly int _dmgTrigger = Animator.StringToHash("ApplyDmg");
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

        public HeroView(Animator hero)
        {
            _hero = hero;
        }

        public void ApplyDmg()
        {
            _hero.SetTrigger(_dmgTrigger);
        }

        public void Teleport()
        {
            _hero.SetTrigger(_glitchTrigger);
        }
    }
}