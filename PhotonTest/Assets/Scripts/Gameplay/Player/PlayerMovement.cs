using Gameplay.Input;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement
    {
        private readonly CharacterController _characterController;
        private readonly IInputService _inputService;
        private readonly PlayerConfig _playerConfig;

        public PlayerMovement(
            CharacterController characterController,
            IInputService inputService,
            PlayerConfig playerConfig)
        {
            _characterController = characterController;
            _inputService = inputService;
            _playerConfig = playerConfig;
        }

        public void Update()
        {
            if (_characterController == null)
            {
                return;
            }

            var moveInput = _inputService.MoveInput;
            var transform = _characterController.transform;
            var moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

            _characterController.Move(moveDirection * _playerConfig.MoveSpeed * Time.deltaTime);
        }
    }
}
