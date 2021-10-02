using UnityEngine;

namespace LD49
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/Hero Config")]
    public class HeroConfigSO : ScriptableObject
    {
        public float JumpForce;
        public float MoveSpeed;
        public float JumpFromWallDuration = 0.15f;
        public float MaxFallSpeed;
    }
}