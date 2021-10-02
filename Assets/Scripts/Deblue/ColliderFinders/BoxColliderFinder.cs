using UnityEngine;

namespace Deblue.ColliderFinders
{
    public class BoxColliderFinder : ColliderFinder
    {
        private readonly Collider2D _collider;
        private readonly Vector3 _offset;
        private readonly Vector2 _direction;
        private readonly int _layerMask;

        public override RaycastHit2D Hit { get; protected set; }

        public override bool IsColliderFound
        {
            get
            {
                var bounds = _collider.bounds;
                Hit = Physics2D.BoxCast(bounds.center + _offset, bounds.size, 0f, _direction, 0, _layerMask);
                return Hit.collider != null;
            }
        }

        public BoxColliderFinder(Collider2D collider, Vector2 direction, int layerMask, Vector3 offset = new Vector3())
        {
            _collider = collider;
            _direction = direction;
            _layerMask = layerMask;
            _offset = offset;
        }

        #region GizmosDrawing

#if UNITY_EDITOR
        protected override Color FoundColor { get; } = Color.green;
        protected override Color NotFoundColor { get; } = Color.red;

        protected override void MyDrawGizmos()
        {
            var bounds = _collider.bounds;
            Gizmos.DrawWireCube(bounds.center + _offset, bounds.size);
        }
#endif

        #endregion
    }
}