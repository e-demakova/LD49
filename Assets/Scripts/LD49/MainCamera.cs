using Deblue.Stats;
using Kino;
using UnityEngine;

namespace Deblue
{
    [DefaultExecutionOrder(-990)]
    public class MainCamera : MonoBehaviour
    {
        public static Camera Camera;
        public static AnalogGlitch Glitch;

        private void Awake()
        {
            Camera = UnityEngine.Camera.main;
            Glitch = GetComponent<AnalogGlitch>();
        }

        public static void SetDieEffect()
        {
            Glitch.colorDrift = 0.5f;
        }
    }
}
