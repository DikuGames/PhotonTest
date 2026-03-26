using Gameplay.Input;
using Infrastructure.Loading.Scene;
using Zenject;

namespace Infrastructure.Installers
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>().To<PhotonSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<StandaloneInput>().AsSingle();
        }
    }
}
