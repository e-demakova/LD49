using UnityEngine;

namespace Deblue.ColliderFinders
{
    public class CircleColliderFinder : ColliderFinder
    {
        private readonly float _radius;
        private readonly Transform _finder;
        private readonly int _layerMack;

        public override RaycastHit2D Hit { get; protected set; }

        public override bool IsColliderFound
        {
            get
            {
                Hit = Physics2D.CircleCast(_finder.position, _radius, Vector2.zero, 0f, _layerMack);
                return Hit.collider;
            }
        }

        public CircleColliderFinder(Transform finder, int layerMack, float radius = 1f)
        {
            _finder = finder;
            _layerMack = layerMack;
            _radius = radius;
        }

        #region GizmosDrawing

#if UNITY_EDITOR
        protected override Color FoundColor { get; } = new Color(0, 1, 0, 0.4f);
        protected override Color NotFoundColor { get; } = new Color(1, 0, 0, 0.4f);

        protected override void MyDrawGizmos()
        {
            Gizmos.DrawWireSphere(_finder.position, _radius);
        }
#endif

        #endregion
    }
}