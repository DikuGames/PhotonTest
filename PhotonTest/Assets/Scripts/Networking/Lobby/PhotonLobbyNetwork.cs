using System;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Networking.Lobby
{
    public sealed class PhotonLobbyNetwork : MonoBehaviourPunCallbacks, ILobbyNetwork
    {
        [SerializeField] private byte _maxPlayersPerRoom = 4;

        private TaskCompletionSource<bool> _joinRandomRoomTask;
        private TaskCompletionSource<bool> _createRoomTask;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public async Task<bool> TryJoinRandomRoomAsync()
        {
            await EnsureConnectedAsync();

            _joinRandomRoomTask = new TaskCompletionSource<bool>();
            PhotonNetwork.JoinRandomRoom();

            return await _joinRandomRoomTask.Task;
        }

        public async Task CreateRoomAsync()
        {
            await EnsureConnectedAsync();

            _createRoomTask = new TaskCompletionSource<bool>();
            var roomOptions = new RoomOptions
            {
                MaxPlayers = _maxPlayersPerRoom
            };

            PhotonNetwork.CreateRoom(null, roomOptions);
            await _createRoomTask.Task;
        }

        public override void OnConnectedToMaster()
        {
            _connectTask?.TrySetResult(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            _joinRandomRoomTask?.TrySetResult(false);
        }

        public override void OnJoinedRoom()
        {
            _joinRandomRoomTask?.TrySetResult(true);
            _createRoomTask?.TrySetResult(true);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _createRoomTask?.TrySetException(new InvalidOperationException($"Create room failed: {message}"));
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            var exception = new InvalidOperationException($"Disconnected: {cause}");

            _connectTask?.TrySetException(exception);
            _joinRandomRoomTask?.TrySetException(exception);
            _createRoomTask?.TrySetException(exception);

            _connectTask = null;
            _joinRandomRoomTask = null;
            _createRoomTask = null;
        }

        private TaskCompletionSource<bool> _connectTask;

        private async Task EnsureConnectedAsync()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                return;
            }

            _connectTask = new TaskCompletionSource<bool>();
            PhotonNetwork.ConnectUsingSettings();

            await _connectTask.Task;
        }
    }
}
