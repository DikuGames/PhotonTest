using Gameplay.StaticData;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Gameplay.Player.Factory
{
    public class PhotonPlayerFactory : IPlayerFactory
    {
        private readonly PlayerConfig _playerConfig;
        private readonly DiContainer _container;

        public PhotonPlayerFactory(IStaticDataService staticDataService, DiContainer container)
        {
            _playerConfig = staticDataService.PlayerConfig;
            _container = container;
        }

        public GameObject Create(Vector3 position, Quaternion rotation)
        {
            var player = PhotonNetwork.Instantiate(_playerConfig.PlayerPrefab.name, position, rotation);
            var playerController = player.GetComponent<PlayerController>();
            _container.InjectGameObject(player);

            if (playerController != null)
            {
                playerController.Initialize(_playerConfig);
            }

            return player;
        }
    }
}
