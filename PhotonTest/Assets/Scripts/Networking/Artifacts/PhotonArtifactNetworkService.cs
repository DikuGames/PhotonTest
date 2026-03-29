using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Gameplay.Artifacts;
using Networking.EventCodes;
using Photon.Pun;
using Photon.Realtime;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Networking.Artifacts
{
    public class PhotonArtifactNetworkService : IArtifactNetworkService, IInitializable, IDisposable, IOnEventCallback, IInRoomCallbacks
    {
        private const string CollectedArtifactIdsKey = "CollectedArtifactIds";

        private readonly ArtifactRegistry _artifactRegistry;
        private HashSet<int> _collectedArtifactIds = new();

        public PhotonArtifactNetworkService(ArtifactRegistry artifactRegistry)
        {
            _artifactRegistry = artifactRegistry;
        }

        public void Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public bool ApplyInitialState()
        {
            if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom == null)
            {
                return false;
            }

            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CollectedArtifactIdsKey))
            {
                TryInitializeRoomState();
                return false;
            }

            ApplyCollectedArtifacts(PhotonNetwork.CurrentRoom.CustomProperties);
            return true;
        }

        public void RequestCollect(int artifactId)
        {
            if (!PhotonNetwork.InRoom)
            {
                TryCollectLocally(artifactId);
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                ConfirmCollect(artifactId);
                return;
            }

            PhotonNetwork.RaiseEvent(
                NetworkEventCodes.ArtifactCollectRequest,
                artifactId,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != NetworkEventCodes.ArtifactCollectRequest)
            {
                return;
            }

            if (PhotonNetwork.IsMasterClient && photonEvent.CustomData is int requestedArtifactId)
            {
                ConfirmCollect(requestedArtifactId);
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            ApplyCollectedArtifacts(propertiesThatChanged);
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        private void ConfirmCollect(int artifactId)
        {
            if (!_artifactRegistry.TryGet(artifactId, out var collectable) || collectable.IsCollected || _collectedArtifactIds.Contains(artifactId))
            {
                return;
            }

            _collectedArtifactIds.Add(artifactId);

            var roomProperties = new Hashtable
            {
                [CollectedArtifactIdsKey] = _collectedArtifactIds.ToArray()
            };

            PhotonNetwork.CurrentRoom?.SetCustomProperties(roomProperties);
        }

        private void TryInitializeRoomState()
        {
            if (!PhotonNetwork.IsMasterClient || PhotonNetwork.CurrentRoom == null)
            {
                return;
            }

            var roomProperties = new Hashtable
            {
                [CollectedArtifactIdsKey] = Array.Empty<int>()
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }

        private void ApplyCollectedArtifacts(Hashtable roomProperties)
        {
            if (roomProperties == null || !roomProperties.TryGetValue(CollectedArtifactIdsKey, out var collectedArtifactIdsRaw))
            {
                return;
            }

            var collectedArtifactIds = new HashSet<int>(collectedArtifactIdsRaw as int[] ?? Array.Empty<int>());
            _collectedArtifactIds = collectedArtifactIds;

            foreach (var artifactId in collectedArtifactIds)
            {
                TryCollectLocally(artifactId);
            }
        }

        private void TryCollectLocally(int artifactId)
        {
            if (!_artifactRegistry.TryGet(artifactId, out var collectable) || collectable.IsCollected)
            {
                return;
            }

            collectable.Collect();
        }
    }
}
