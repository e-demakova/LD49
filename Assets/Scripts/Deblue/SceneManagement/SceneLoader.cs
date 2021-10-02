using System;
using System.Threading.Tasks;
using Deblue.ObservingSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Deblue.SceneManagement
{
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

    public class SceneLoader
    {
        public SceneSO CurrentScene;
        public SceneSO PreviousScene;

        private readonly Handler<SceneLoadingStarted> _sceneLoadingStarted = new Handler<SceneLoadingStarted>();
        private readonly Handler<SceneLoaded> _sceneLoaded = new Handler<SceneLoaded>();

        private SceneSO _sceneToLoad;
        private SceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;

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
            {
                return;
            }

            _sceneLoadingStarted.Raise(new SceneLoadingStarted());

            _sceneToLoad = sceneToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            PreviousScene = CurrentScene;
            CurrentScene = sceneToLoad;

            UnloadPreviousScene();
            LoadNextScene();
        }

        private void LoadPersistentScenes(SceneSO[] scenes)
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                LoadScene(scenes[i].AssetRef);
            }
        }

        private void UnloadPreviousScene()
        {
            if (_currentlyLoadedScene == null)
            {
                return;
            }

            if (_currentlyLoadedScene.AssetRef.OperationHandle.IsValid())
            {
                _currentlyLoadedScene.AssetRef.UnLoadScene();
            }
        }

        private void LoadNextScene()
        {
            if (_showLoadingScreen)
            {
            }

            LoadScene(_sceneToLoad.AssetRef, SetActiveScene);
        }

        private void LoadScene(AssetReference assetRef, Action<SceneInstance> action)
        {
            var task = LoadSceneAsync(assetRef, action);
        }

        private async Task LoadSceneAsync(AssetReference assetRef, Action<SceneInstance> action)
        {
            var operation = assetRef.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            await operation.Task;
            action.Invoke(operation.Result);
        }

        private void LoadScene(AssetReference assetRef)
        {
            var task = LoadSceneAsync(assetRef);
        }

        private async Task LoadSceneAsync(AssetReference assetRef)
        {
            await assetRef.LoadSceneAsync(LoadSceneMode.Additive, true, 0).Task;
        }

        private void SetActiveScene(SceneInstance sceneInstance)
        {
            SceneManager.SetActiveScene(sceneInstance.Scene);
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