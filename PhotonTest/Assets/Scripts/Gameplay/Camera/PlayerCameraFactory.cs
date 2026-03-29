using Gameplay.Player;
using Gameplay.StaticData;
using UnityEngine;

namespace Gameplay.Camera
{
    public class PlayerCameraFactory : IPlayerCameraFactory
    {
        private readonly PlayerConfig _playerConfig;

        public PlayerCameraFactory(IStaticDataService staticDataService)
        {
            _playerConfig = staticDataService.PlayerConfig;
        }

        public GameObject Create(Transform parent)
        {
            if (_playerConfig == null || _playerConfig.CameraPrefab == null || parent == null)
            {
                return null;
            }

            return Object.Instantiate(_playerConfig.CameraPrefab, parent);
        }
    }
}
