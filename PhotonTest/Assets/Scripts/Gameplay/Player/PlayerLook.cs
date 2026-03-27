using Gameplay.Input;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerLook
    {
        private readonly Transform _bodyTransform;
        private readonly Transform _cameraTransform;
        private readonly IInputService _inputService;
        private readonly PlayerConfig _playerConfig;

        private float _pitch;

        public PlayerLook(
            Transform bodyTransform,
            Transform cameraTransform,
            IInputService inputService,
            PlayerConfig playerConfig)
        {
            _bodyTransform = bodyTransform;
            _cameraTransform = cameraTransform;
            _inputService = inputService;
            _playerConfig = playerConfig;
        }

        public void Update()
        {
            if (_bodyTransform == null || _cameraTransform == null)
            {
                return;
            }

            var lookInput = _inputService.LookInput.normalized;

            _bodyTransform.Rotate(Vector3.up * (lookInput.x * _playerConfig.LookSensitivity * Time.deltaTime));

            _pitch -= lookInput.y * _playerConfig.LookSensitivity * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, _playerConfig.MinPitch, _playerConfig.MaxPitch);

            _cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
