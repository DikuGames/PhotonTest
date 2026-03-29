using System.Threading.Tasks;
using Gameplay.EntryPoint;
using Gameplay.Player.Factory;
using Networking.Artifacts;
using Photon.Pun;

namespace Gameplay.StateMachine.States
{
    public class PrepareGameState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly IPlayerFactory _playerFactory;
        private readonly IArtifactNetworkService _artifactNetworkService;
        private readonly GameEntryPoint _gameEntryPoint;

        private bool _isActive;

        public PrepareGameState(
            IStateSwitcher stateSwitcher,
            IPlayerFactory playerFactory,
            IArtifactNetworkService artifactNetworkService,
            GameEntryPoint gameEntryPoint)
        {
            _stateSwitcher = stateSwitcher;
            _playerFactory = playerFactory;
            _artifactNetworkService = artifactNetworkService;
            _gameEntryPoint = gameEntryPoint;
        }

        public void Enter()
        {
            _isActive = true;
            PrepareAsync();
        }

        public void Exit()
        {
            _isActive = false;
        }

        private async void PrepareAsync()
        {
            while (_isActive && (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady || PhotonNetwork.CurrentRoom == null))
            {
                await Task.Yield();
            }

            while (_isActive && !_artifactNetworkService.ApplyInitialState())
            {
                await Task.Yield();
            }

            if (!_isActive)
            {
                return;
            }

            var spawnPoint = _gameEntryPoint.GetSpawnPoint();
            _playerFactory.Create(spawnPoint.position, spawnPoint.rotation);
            _stateSwitcher.SwitchState<GameState>();
        }
    }
}
