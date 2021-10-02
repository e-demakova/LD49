using LD49.Hero;
using UnityEngine;

namespace LD49.Enviroment
{
    public interface ITeleportable
    {
        void Teleport(Vector2 position);
    }
    
    [RequireComponent(typeof(Collider2D))]
    public class TeleportTrap : Trap
    {
        [SerializeField] private Transform[] _teleportPoints;
        
        protected override void ActivateTrap(HeroController hero)
        {
            hero.Teleport(_teleportPoints[Random.Range(0, _teleportPoints.Length)].position);
        }
    }
}
