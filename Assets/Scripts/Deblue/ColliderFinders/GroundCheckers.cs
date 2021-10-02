using UnityEngine;

namespace Deblue.ColliderFinders
{
    public class BoxGroundChecker : BoxColliderFinder, IGroundChecker
    {
        public Vector2 GroundNormal { get; }
        
        public BoxGroundChecker(Collider2D collider, Vector2 direction, Vector3 offset = new Vector3()) 
            : base(collider, direction, LayerMasks.Ground, offset)
        {
            GroundNormal = -direction;
        }
    }
    
    public class RayGroundChecker : RayColliderFinder, IGroundChecker
    {
        public Vector2 GroundNormal { get; }

        public RayGroundChecker(Transform transform, Vector2 direction, float distance, Vector3 offset = new Vector3()) 
            : base(transform, direction, distance, LayerMasks.Ground,  offset)
        {
            GroundNormal = -direction;
        }
    }
}