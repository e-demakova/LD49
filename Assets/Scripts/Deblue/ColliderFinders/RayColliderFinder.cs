using UnityEngine;

namespace Deblue.ColliderFinders
{
    public class RayColliderFinder : ColliderFinder
    {
        private readonly Transform _transform;
        private readonly Vector2 _direction;
        private readonly Vector3 _offset;
        private readonly float _distance;
        private readonly int _layerMask;

        public override RaycastHit2D Hit { get; protected set; }

        public override bool IsColliderFound
        {
            get
            {
                Hit = Physics2D.Raycast(_transform.position + _offset, _direction, _distance, _layerMask);
                return Hit.collider != null;
            }
        }

        public RayColliderFinder(Transform transform, Vector2 direction, float distance, int layerMask, Vector3 offset = new Vector3())
        {
            _transform = transform;
            _direction = direction;
            _distance = distance;
            _layerMask = layerMask;
            _offset = offset;
        }

        #region GizmosDrawing

#if UNITY_EDITOR
        protected override Color FoundColor { get; } = Color.green;
        protected override Color NotFoundColor { get; } = Color.red;

        protected override void MyDrawGizmos()
        {
            Gizmos.DrawRay(_transform.position + _offset, _direction * _distance);
        }
#endif

        #endregion
    }
}