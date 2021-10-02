using UnityEngine;

namespace Deblue.Input
{
    public readonly struct ButtonDown
    {
        public readonly KeyCode Code;

        public ButtonDown(KeyCode code)
        {
            Code = code;
        }
    }

    public readonly struct OnButton
    {
        public readonly KeyCode Code;

        public OnButton(KeyCode code)
        {
            Code = code;
        }
    }

    public readonly struct ButtonUp
    {
        public readonly KeyCode Code;

        public ButtonUp(KeyCode code)
        {
            Code = code;
        }
    }

    public readonly struct InputDirection
    {
        public readonly Vector2Int Axis;

        public InputDirection(Vector2Int axis)
        {
            Axis = axis;
        }
    }

    public readonly struct MousePosition
    {
        public readonly Vector2 Position;

        public MousePosition(Vector2 axis)
        {
            Position = axis;
        }
    }
}