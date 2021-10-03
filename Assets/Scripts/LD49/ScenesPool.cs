using System;
using System.Collections.Generic;
using Deblue.SceneManagement;
using Random = UnityEngine.Random;

namespace LD49
{
    public class ScenesPool : IDisposable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly StartScenesConfigSO _config;

        private readonly List<SceneSO> _firstLevels = new List<SceneSO>(5);
        private readonly List<SceneSO> _midLevels = new List<SceneSO>(10);

        private int _levelNumber;

        public ScenesPool(StartScenesConfigSO config, SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _config = config;

            ReloadScenes();
            _sceneLoader.SceneLoaded.Subscribe(ReloadScenes);
        }

        public void Dispose()
        {
            _sceneLoader.SceneLoaded.Unsubscribe(ReloadScenes);
        }

        public void LoadNextLevel()
        {
            _levelNumber++;
            _sceneLoader.LoadNextScene(GetRandomLevel());
        }

        private void ReloadScenes(SceneLoaded context)
        {
            if (context.NewScene.Type == SceneType.Shop)
                ReloadScenes();
        }

        private void ReloadScenes()
        {
            _levelNumber = 0;

            _midLevels.Clear();
            _firstLevels.Clear();

            _firstLevels.AddRange(_config.FirstLevels);
            _midLevels.AddRange(_config.MidLevels);
        }

        private SceneSO GetRandomLevel()
        {
            if (_midLevels.Count == 0)
                _midLevels.AddRange(_config.MidLevels);

            return GetRandomSceneFromList(_config.FirstLevelsCount >= _levelNumber ? _firstLevels : _midLevels);
        }

        private SceneSO GetRandomSceneFromList(List<SceneSO> list)
        {
            var scene = list[Random.Range(0, list.Count)];
            list.Remove(scene);
            return scene;
        }
    }
}