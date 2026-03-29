using System;
using System.Collections.Generic;
using Infrastructure.Loading.Scene;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace Networking.Room
{
    public class PhotonRoomExitService : IRoomExitService, IInitializable, IDisposable, IMatchmakingCallbacks
    {
        private readonly ISceneLoader _sceneLoader;
        private bool _isExiting;

        public PhotonRoomExitService(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void ExitToLobby()
        {
            if (_isExiting)
            {
                return;
            }

            _isExiting = true;

            if (!PhotonNetwork.InRoom)
            {
                _sceneLoader.Load(SceneNames.Lobby);
                _isExiting = false;
                return;
            }

            PhotonNetwork.LeaveRoom();
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnJoinedRoom()
        {
            _isExiting = false;
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        public void OnLeftRoom()
        {
            _sceneLoader.Load(SceneNames.Lobby);
            _isExiting = false;
        }
    }
}
