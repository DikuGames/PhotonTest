using System.Collections;
using Gameplay.Player.Factory;
using Networking.Artifacts;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Gameplay.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;

        private IPlayerFactory _playerFactory;
        private IArtifactNetworkService _artifactNetworkService;
        private bool _isInitialized;

        [Inject]
        private void Construct(IPlayerFactory playerFactory, IArtifactNetworkService artifactNetworkService)
        {
            _playerFactory = playerFactory;
            _artifactNetworkService = artifactNetworkService;
        }

        private void Start()
        {
            if (_isInitialized)
            {
                return;
            }

            StartCoroutine(InitializeGameScene());
        }

        private IEnumerator InitializeGameScene()
        {
            while (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady || PhotonNetwork.CurrentRoom == null)
            {
                yield return null;
            }

            _artifactNetworkService.ApplyInitialState();
            yield return null;

            if (_isInitialized)
            {
                yield break;
            }

            var spawnPoint = GetSpawnPoint();
            _playerFactory.Create(spawnPoint.position, spawnPoint.rotation);
            _isInitialized = true;
        }

        private Transform GetSpawnPoint()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                return transform;
            }

            var randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex] != null ? _spawnPoints[randomIndex] : transform;
        }
    }
}
