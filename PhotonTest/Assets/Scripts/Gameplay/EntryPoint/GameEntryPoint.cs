using Gameplay.Player.Factory;
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
