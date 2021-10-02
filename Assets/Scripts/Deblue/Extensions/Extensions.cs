using System;
using UnityEngine;

namespace Deblue.Extensions
{
    public static class TransformExtensions
    {
        public static Quaternion Tern(this Transform transform, float value)
        {
            if (value == 0f)
                return transform.rotation;
            
            int tern = value < 0f ? 180 : 0;
            transform.rotation = Quaternion.Euler(0, tern, 0);

            return transform.rotation;
        }
        
        public static void MoveHorizontal(this Rigidbody2D rigidbody, int direction, float speed)
        {
            if (direction == 0)
                return;

            rigidbody.Move(Vector2.right * direction, speed);
        }
        
        public static void MoveVertical(this Rigidbody2D rigidbody, int direction, float speed)
        {
            if (direction == 0)
                return;

            rigidbody.Move(Vector2.up * direction, speed);
        }
        
        public static void Move(this Rigidbody2D rigidbody, Vector2 direction, float speed)
        {
            var deltaPosition = direction * speed * Time.fixedDeltaTime;
            rigidbody.position += deltaPosition;
        }

        public static void MoveVertical(this Rigidbody2D rigidbody, float newYPosition)
        {
            var newPosition = new Vector2(rigidbody.position.x, newYPosition);
            rigidbody.position = newPosition;
        }
        
        public static void MoveHorizontal(this Rigidbody2D rigidbody, float newXPosition)
        {
            var newPosition = new Vector2(newXPosition, rigidbody.position.y);
            rigidbody.position = newPosition;
        }
    }

    public static class ObjectExtensions
    {
        public static T Do<T>(this T obj, Action<T> action, bool when)
        {
            if (when)
                action.Invoke(obj);

            return obj;
        }

        public static T Do<T>(this T obj, Action<T> action)
        {
            action.Invoke(obj);
            return obj;
        }
    }

    public static class MathExtensions
    {
        public static float CalculatePercent(this float value, float upperLimit)
        {
            return value / upperLimit;
        }

        public static int GetClearDirection(this float direction)
        {
            var i = 0;
            if (direction != 0)
            {
                i = direction > 0 ? 1 : -1;
            }

            return i;
        }

        public static int GetClearDirection(this float direction, float threshold)
        {
            var i = 0;
            if (direction > threshold)
            {
                i = 1;
            }
            else if (direction < -threshold)
            {
                i = -1;
            }

            return i;
        }
    }
}