using System;
using System.Collections.Generic;
using Infrastructure.Loading.Scene;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Zenject;

namespace Networking.Connection
{
    public class PhotonConnectionRecoveryService : IConnectionCallbacks, IInitializable, IDisposable
    {
        private readonly ISceneLoader _sceneLoader;
        private bool _isHandlingDisconnect;

        public PhotonConnectionRecoveryService(ISceneLoader sceneLoader)
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

        public void OnDisconnected(DisconnectCause cause)
        {
            if (_isHandlingDisconnect)
            {
                return;
            }

            if (SceneManager.GetActiveScene().name == SceneNames.Lobby)
            {
                return;
            }

            _isHandlingDisconnect = true;
            _sceneLoader.Load(SceneNames.Lobby);
            _isHandlingDisconnect = false;
        }

        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }
    }
}
