using Deblue.Battle;
using Deblue.Extensions;
using Deblue.ObservingSystem;
using UnityEngine;

namespace LD49
{
    public readonly struct PlayerOnTheGround
    {
    }

    public class HeroModel
    {
        public const AttackerType AttackerType = Deblue.Battle.AttackerType.Player;
        
        public float JumpForce;
        public float JumpFromWallDuration;
        public float MoveSpeed;
        public float MaxFallSpeed;
        public float MaxHp;
        public float Hp;
        public int MaxJumps = 1;
        public int JumpsMade;
        public int HorizontalMoveDirection;

        public bool IsWallNear;
        public bool IsMoveLock;
        public bool IsInvincible;

        public Vector2Int InputDirection;
        public Vector2 WallNormal;
        public Vector2 Velocity;
        public Vector2 Position;

        private bool _isGrounded;

        private readonly Handler<PlayerOnTheGround> _onGrounded = new Handler<PlayerOnTheGround>();

        public bool IsCanJumpFromGround => MaxJumps - JumpsMade > 0;
        public bool IsCanJumpFromWall => IsWallNear && !IsGrounded;
        public bool IsLeaningOnWall => IsWallNear && !IsGrounded && InputDirection.x == (int) - WallNormal.x;
        public IReadOnlyHandler<PlayerOnTheGround> OnGrounded => _onGrounded;

        public Vector2Int MoveDirection
        {
            get
            {
                int x = HorizontalMoveDirection;
                int y = Velocity.y.GetClearDirection();

                return new Vector2Int(x, y);
            }
        }

        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                if (_isGrounded == value)
                    return;
                
                _isGrounded = value;
                if (_isGrounded) 
                    _onGrounded.Raise(new PlayerOnTheGround());
            }
        }


        public HeroModel(HeroConfigSO config)
        {
            MaxFallSpeed = config.MaxFallSpeed;
            JumpFromWallDuration = config.JumpFromWallDuration;
            MoveSpeed = config.MoveSpeed;
            JumpForce = config.JumpForce;
        }
    }
}