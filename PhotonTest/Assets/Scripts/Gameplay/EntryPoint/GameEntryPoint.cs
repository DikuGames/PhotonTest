using UnityEngine;

namespace Gameplay.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;

        //Упрещенный вариант, без проверки занятости точки
        public Transform GetSpawnPoint()
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
