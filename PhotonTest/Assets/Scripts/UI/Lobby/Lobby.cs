using Infrastructure.Loading.Scene;
using Networking.Lobby;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private LobbyView _view;
        [SerializeField] private MonoBehaviour _lobbyNetworkSource;

        private ILobbyNetwork _lobbyNetwork;
        private ISceneLoader _sceneLoader;
        private bool _isHandlingConnect;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _lobbyNetwork = _lobbyNetworkSource as ILobbyNetwork;

            if (_lobbyNetwork == null)
            {
                Debug.LogError("Lobby dependencies are not configured.", this);
                enabled = false;
                return;
            }

            _view.OnConnectClicked += OnConnectClicked;
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                _view.OnConnectClicked -= OnConnectClicked;
            }
        }

        private async void OnConnectClicked()
        {
            if (_isHandlingConnect)
            {
                return;
            }

            _isHandlingConnect = true;

            try
            {
                var joinedExistingRoom = await _lobbyNetwork.TryJoinRandomRoomAsync();

                if (!joinedExistingRoom)
                {
                    await _lobbyNetwork.CreateRoomAsync();
                    _sceneLoader.Load(SceneNames.Game);
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Lobby connection failed: {exception.Message}", this);
            }
            finally
            {
                _isHandlingConnect = false;
            }
        }
    }
}
