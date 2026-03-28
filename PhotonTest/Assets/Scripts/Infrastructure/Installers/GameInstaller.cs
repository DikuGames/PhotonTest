using Gameplay.Artifacts;
using Gameplay.Player.Factory;
using Networking.Artifacts;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ArtifactRegistry _artifactRegistry;

        public override void InstallBindings()
        {
            BindNetworking();
            BindServices();
            
            Container.Bind<IPlayerFactory>().To<PhotonPlayerFactory>().AsSingle();
        }

        private void BindNetworking()
        {
            Container.BindInterfacesAndSelfTo<PhotonArtifactNetworkService>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ArtifactRegistry>().FromInstance(_artifactRegistry).AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactCollectionService>().AsSingle();
        }
    }
}
