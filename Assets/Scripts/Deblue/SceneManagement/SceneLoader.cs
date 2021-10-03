using System;
using System.Threading.Tasks;
using Deblue.ObservingSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deblue.SceneManagement
{
    public interface ISceneLoader
    {
       void LoadNextScene(SceneSO sceneToLoad, bool showLoadingScreen = false);
    }
    
    public readonly struct SceneLoaded
    {
        public readonly SceneSO NewScene;
        public readonly SceneSO PreviousScene;

        public SceneLoaded(SceneSO newScene, SceneSO previousScene)
        {
            NewScene = newScene;
            PreviousScene = previousScene;
        }
    }

    public readonly struct SceneLoadingStarted
    {
    }

    public class SceneLoader : ISceneLoader
    {
        public SceneSO CurrentScene;
        public SceneSO PreviousScene;

        private readonly Handler<SceneLoadingStarted> _sceneLoadingStarted = new Handler<SceneLoadingStarted>();
        private readonly Handler<SceneLoaded> _sceneLoaded = new Handler<SceneLoaded>();

        private SceneSO _sceneToLoad;
        private SceneSO _currentlyLoadedScene;

        private bool _isLoading;

        public IReadOnlyHandler<SceneLoaded> SceneLoaded => _sceneLoaded;
        public IReadOnlyHandler<SceneLoadingStarted> SceneLoadingStarted => _sceneLoadingStarted;

        public SceneLoader(StartScenesConfigSO scenes)
        {
            if (!scenes.LoadSettedScenes)
                return;

            LoadNextScene(scenes.FirstScene);
            LoadPersistentScenes(scenes.PersistentGameStartScenes);
        }

        public void LoadNextScene(SceneSO sceneToLoad, bool showLoadingScreen = false)
        {
            if (_isLoading)
                return;

            if (showLoadingScreen)
            {
            }
            
            _sceneLoadingStarted.Raise(new SceneLoadingStarted());

            _sceneToLoad = sceneToLoad;
            _isLoading = true;

            PreviousScene = CurrentScene;
            CurrentScene = sceneToLoad;

            UnloadPreviousScene();

            SceneManager.LoadSceneAsync(_sceneToLoad.Name, LoadSceneMode.Additive);
            SetActiveScene();
        }

        private void LoadPersistentScenes(SceneSO[] scenes)
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                SceneManager.LoadSceneAsync(scenes[i].Name, LoadSceneMode.Additive);
            }
        }

        private void UnloadPreviousScene()
        {
            if (_currentlyLoadedScene == null)
                return;

            SceneManager.UnloadSceneAsync(_currentlyLoadedScene.Name);
        }
        
        private void SetActiveScene()
        {
            _currentlyLoadedScene = _sceneToLoad;
            _sceneLoaded.Raise(new SceneLoaded(CurrentScene, PreviousScene));

            _isLoading = false;
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}