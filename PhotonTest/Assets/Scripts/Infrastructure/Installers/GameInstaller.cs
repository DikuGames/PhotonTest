using Gameplay.Player.Factory;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerFactory>().To<PhotonPlayerFactory>().AsSingle();
        }
    }
}
