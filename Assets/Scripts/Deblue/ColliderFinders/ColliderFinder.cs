using UnityEngine;

namespace Deblue.ColliderFinders
{
    public interface IColliderFinder
    {
        bool IsColliderFound { get; }
        RaycastHit2D Hit { get; }

        #region GizmosDrawing

#if UNITY_EDITOR
        void DrawGizmos(bool inverseColor = false);
#endif

        #endregion
    }

    public interface IGroundChecker : IColliderFinder
    {
        Vector2 GroundNormal { get; }
    }

    public abstract class ColliderFinder : IColliderFinder
    {
        public abstract bool IsColliderFound { get; }
        public abstract RaycastHit2D Hit { get; protected set; }

#if UNITY_EDITOR

        protected abstract Color FoundColor { get; }
        protected abstract Color NotFoundColor { get; }

        public void DrawGizmos(bool inverseColor = false)
        {
            var foundColor = inverseColor ? NotFoundColor : FoundColor;
            var notFoundColor = inverseColor ? FoundColor : NotFoundColor;

            Gizmos.color = IsColliderFound ? foundColor : notFoundColor;

            MyDrawGizmos();
        }

        protected abstract void MyDrawGizmos();
#endif
    }
}