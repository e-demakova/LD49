using Deblue.Data;
using Deblue.Data.Localization;
using Deblue.Input;
using Deblue.SceneManagement;
using Deblue.Story.DialogSystem;
using Deblue.Story.Steps;
using LD49.Enviroment;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace LD49.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private StartScenesConfigSO _scenes;
        
        public override void InstallBindings()
        {
            InstallServices();
        }

        private void InstallServices()
        {
            Container.BindInterfacesAndSelfTo<InputReceiver>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<LoadService>().FromInstance(new LoadService()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<StartScenesConfigSO>().FromInstance(_scenes).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<WorldStats>().AsSingle().NonLazy();
        }
    }
}