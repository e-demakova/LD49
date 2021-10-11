using Deblue.Stats;
using UnityEngine;

namespace Deblue
{
    [DefaultExecutionOrder(-990)]
    public class MainCamera : MonoBehaviour
    {
        public static Camera Camera;

        private void Awake()
        {
            Camera = UnityEngine.Camera.main;
        }

    }
}
