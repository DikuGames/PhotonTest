using Gameplay.Input;
using Gameplay.StaticData;
using Networking.Artifacts;
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
        [SerializeField] private PlayerDetector _playerDetector;

        private PlayerConfig _playerConfig;
        private IInputService _inputService;
        private IArtifactNetworkService _artifactNetworkService;
        private PlayerMovement _playerMovement;
        private PlayerLook _playerLook;
        private PlayerInteraction _playerInteraction;
        private PhotonView _photonView;

        [Inject]
        public void Construct(
            IInputService inputService,
            IStaticDataService staticDataService,
            IArtifactNetworkService artifactNetworkService)
        {
            _inputService = inputService;
            _artifactNetworkService = artifactNetworkService;
            _playerConfig = staticDataService.PlayerConfig;
        }

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
            {
                _cameraTransform.gameObject.SetActive(false);
                return;
            }

            _playerInteraction = new PlayerInteraction(_cameraTransform, _inputService, _artifactNetworkService, _playerConfig);
            _playerDetector?.Initialize(_inputService, _playerConfig);
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

        private void OnDestroy()
        {
            _playerInteraction?.Dispose();
        }
    }
}
