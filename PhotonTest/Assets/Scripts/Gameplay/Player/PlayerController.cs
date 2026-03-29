using Gameplay.Camera;
using Gameplay.Input;
using Gameplay.StaticData;
using Networking.Artifacts;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerController : MonoBehaviour, IPunObservable
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private PlayerDetector _playerDetector;

        private PlayerConfig _playerConfig;
        private IInputService _inputService;
        private IArtifactNetworkService _artifactNetworkService;
        private IPlayerCameraFactory _playerCameraFactory;
        private PlayerMovement _playerMovement;
        private PlayerLook _playerLook;
        private PlayerInteraction _playerInteraction;
        private PhotonView _photonView;
        private GameObject _localCamera;

        [Inject]
        public void Construct(
            IInputService inputService,
            IStaticDataService staticDataService,
            IArtifactNetworkService artifactNetworkService,
            IPlayerCameraFactory playerCameraFactory)
        {
            _inputService = inputService;
            _artifactNetworkService = artifactNetworkService;
            _playerCameraFactory = playerCameraFactory;
            _playerConfig = staticDataService.PlayerConfig;
        }

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _playerDetector?.Initialize(_playerConfig);

            if (!_photonView.IsMine)
            {
                return;
            }

            _localCamera = _playerCameraFactory.Create(_cameraTransform);
            _playerInteraction = new PlayerInteraction(_cameraTransform, _inputService, _artifactNetworkService, _playerConfig);
            _playerDetector?.BindInput(_inputService);
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_playerDetector.IsActive);
                return;
            }

            var isDetectorActive = (bool)stream.ReceiveNext();
            _playerDetector.SetActive(isDetectorActive);
        }

        private void OnDestroy()
        {
            _playerInteraction?.Dispose();
            _playerDetector?.Dispose();

            if (_localCamera != null)
            {
                Destroy(_localCamera);
            }
        }
    }
}
