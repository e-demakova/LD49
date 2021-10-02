using UnityEngine;

namespace Deblue.SceneManagement
{
    public enum SceneType
    {
        UI,
        Shop,
        Level
    }
    
    [CreateAssetMenu(fileName = "Scene", menuName = "Scenes/Scene")]
    public class SceneSO : ScriptableObject
    {
        public string Name;
        public SceneType Type;
    }
}