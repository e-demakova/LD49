using UnityEngine;

namespace Deblue.SceneManagement
{
    [CreateAssetMenu(fileName = "StartScenes", menuName = "Configs/Start Scenes")]
    public class StartScenesConfigSO : ScriptableObject
    {
        public bool LoadSettedScenes;
        public SceneSO[] PersistentGameStartScenes;
        public SceneSO FirstScene;
    }
}