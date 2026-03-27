using Gameplay.Input;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _cameraTransform;

        private PlayerConfig _playerConfig;
        private IInputService _inputService;
        private PlayerMovement _playerMovement;
        private PlayerLook _playerLook;
        private PhotonView _photonView;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
            {
                _cameraTransform.gameObject.SetActive(false);
            }
            
            _playerMovement = new PlayerMovement(_characterController, _inputService, _playerConfig);
            _playerLook = new PlayerLook(transform, _cameraTransform, _inputService, _playerConfig);
        }

        private void Update()
        {
            if (_playerLook == null || _playerMovement == null)
            {
                return;
            }

            if (_photonView != null && !_photonView.IsMine)
            {
                return;
            }
        
            _playerLook.Update();
            _playerMovement.Update();
        }
    }
}
