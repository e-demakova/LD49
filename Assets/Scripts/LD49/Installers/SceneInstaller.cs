using Deblue.InteractiveObjects;
using LD49.Hero;
using UnityEngine;
using Zenject;

namespace LD49.Installers
{
    public class SceneInstaller : MonoInstaller<SceneInstaller>
    {
        [SerializeField] private HeroController _hero;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSender>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HeroController>().FromInstance(_hero).AsSingle().NonLazy();
        }
    }
}