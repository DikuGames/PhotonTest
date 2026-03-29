using System.Collections;
using Gameplay.Player.Factory;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Gameplay.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;

        private IPlayerFactory _playerFactory;
        private bool _isInitialized;

        [Inject]
        private void Construct(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        private void Start()
        {
            if (_isInitialized)
            {
                return;
            }

            StartCoroutine(SpawnPlayerWhenReady());
        }

        private IEnumerator SpawnPlayerWhenReady()
        {
            while (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady)
            {
                yield return null;
            }

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
