using Gameplay.Input;
using Gameplay.StaticData;
using Infrastructure.Loading.Scene;
using Networking.Connection;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindInput();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<StandaloneInput>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.Bind<ISceneLoader>().To<PhotonSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PhotonConnectionRecoveryService>().AsSingle().NonLazy();
        }
    }
}
