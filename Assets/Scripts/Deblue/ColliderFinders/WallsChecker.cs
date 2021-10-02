using Deblue.ColliderFinders;
using UnityEngine;

namespace DSlacker.ColliderFinders
{
    public class WallsChecker : IGroundChecker
    {
        public Vector2 GroundNormal { get; private set; }

        private readonly RayGroundChecker _leftSide;
        private readonly RayGroundChecker _rightSide;
        public bool IsColliderFound => CheckIsGrounded(_leftSide) || CheckIsGrounded(_rightSide);
        public RaycastHit2D Hit => _leftSide.Hit ? _leftSide.Hit : _rightSide.Hit;

        public WallsChecker(Collider2D collider)
        {
            var bounds = collider.bounds;
            var transform = collider.transform;
            _leftSide = new RayGroundChecker(transform, Vector2.left, bounds.extents.x + 0.04f);
            _rightSide = new RayGroundChecker(transform, Vector2.right, bounds.extents.x + 0.04f);
        }

        private bool CheckIsGrounded(IGroundChecker checker)
        {
            if (checker.IsColliderFound)
            {
                GroundNormal = checker.GroundNormal;
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        public void DrawGizmos(bool inverseColor = false)
        {
            _leftSide.DrawGizmos(inverseColor);
            _rightSide.DrawGizmos(inverseColor);
        }
#endif
    }
}