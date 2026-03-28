using System;
using ExitGames.Client.Photon;
using Gameplay.Artifacts;
using Networking.EventCodes;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace Networking.Artifacts
{
    public class PhotonArtifactNetworkService : IArtifactNetworkService, IInitializable, IDisposable, IOnEventCallback
    {
        private readonly ArtifactRegistry _artifactRegistry;

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
            switch (photonEvent.Code)
            {
                case NetworkEventCodes.ArtifactCollectRequest:
                    if (PhotonNetwork.IsMasterClient && photonEvent.CustomData is int requestedArtifactId)
                    {
                        ConfirmCollect(requestedArtifactId);
                    }
                    break;

                case NetworkEventCodes.ArtifactCollectConfirmed:
                    if (photonEvent.CustomData is int confirmedArtifactId)
                    {
                        TryCollectLocally(confirmedArtifactId);
                    }
                    break;
            }
        }

        private void ConfirmCollect(int artifactId)
        {
            if (!_artifactRegistry.TryGet(artifactId, out var collectable) || collectable.IsCollected)
            {
                return;
            }

            PhotonNetwork.RaiseEvent(
                NetworkEventCodes.ArtifactCollectConfirmed,
                artifactId,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                SendOptions.SendReliable);
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
