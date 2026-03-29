using Gameplay.Artifacts;
using Gameplay.Camera;
using Gameplay.Match;
using Gameplay.Player.Factory;
using Networking.Artifacts;
using Networking.Room;
using UI.Game;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ArtifactRegistry _artifactRegistry;
        [SerializeField] private GameHudView _gameHudView;

        public override void InstallBindings()
        {
            BindNetworking();
            BindServices();
            BindFactories();
            BindUi();
        }

        private void BindNetworking()
        {
            Container.BindInterfacesAndSelfTo<PhotonArtifactNetworkService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PhotonRoomExitService>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ArtifactRegistry>().FromInstance(_artifactRegistry).AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactCollectionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchFinishService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.Bind<IPlayerFactory>().To<PhotonPlayerFactory>().AsSingle();
            Container.Bind<IPlayerCameraFactory>().To<PlayerCameraFactory>().AsSingle();
        }

        private void BindUi()
        {
            Container.Bind<GameHudView>().FromInstance(_gameHudView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameHud>().AsSingle().NonLazy();
        }
    }
}
