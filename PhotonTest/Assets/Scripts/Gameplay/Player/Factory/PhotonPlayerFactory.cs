using Gameplay.StaticData;
using Photon.Pun;
using UnityEngine;

namespace Gameplay.Player.Factory
{
    public class PhotonPlayerFactory : IPlayerFactory
    {
        private readonly PlayerConfig _playerConfig;

        public PhotonPlayerFactory(IStaticDataService staticDataService)
        {
            _playerConfig = staticDataService.PlayerConfig;
        }

        public GameObject Create(Vector3 position, Quaternion rotation)
        {
            return PhotonNetwork.Instantiate(_playerConfig.PlayerPrefab.name, position, rotation);
        }
    }
}
