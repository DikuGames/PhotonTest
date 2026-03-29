using Gameplay.Artifacts;
using Gameplay.Camera;
using Gameplay.EntryPoint;
using Gameplay.Match;
using Gameplay.Player.Factory;
using Gameplay.StateMachine;
using Gameplay.StateMachine.Factory;
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
        [SerializeField] private GameEntryPoint _gameEntryPoint;

        public override void InstallBindings()
        {
            BindNetworking();
            BindServices();
            BindFactories();
            BindStateMachine();
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
            Container.Bind<GameEntryPoint>().FromInstance(_gameEntryPoint).AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactCollectionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchFinishService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.Bind<IPlayerFactory>().To<PhotonPlayerFactory>().AsSingle();
            Container.Bind<IPlayerCameraFactory>().To<PlayerCameraFactory>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
        }

        private void BindUi()
        {
            Container.Bind<GameHudView>().FromInstance(_gameHudView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameHud>().AsSingle().NonLazy();
        }
    }
}
