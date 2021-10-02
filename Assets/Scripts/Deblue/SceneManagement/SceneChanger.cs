using UnityEngine;
using Zenject;

namespace Deblue.SceneManagement
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private SceneSO _nextScene;

        private SceneLoader _loader;

        [Inject]
        public void Construct(SceneLoader loader)
        {
            _loader = loader;
        }

        public void LoadScene()
        {
            _loader.LoadNextScene(_nextScene);
        }
    }
}