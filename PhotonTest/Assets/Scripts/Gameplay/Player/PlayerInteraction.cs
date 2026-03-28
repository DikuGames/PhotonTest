using System;
using Gameplay.Artifacts;
using Gameplay.Input;
using Networking.Artifacts;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerInteraction : IDisposable
    {
        private readonly Transform _cameraTransform;
        private readonly IInputService _inputService;
        private readonly IArtifactNetworkService _artifactNetworkService;
        private readonly PlayerConfig _playerConfig;

        public PlayerInteraction(
            Transform cameraTransform,
            IInputService inputService,
            IArtifactNetworkService artifactNetworkService,
            PlayerConfig playerConfig)
        {
            _cameraTransform = cameraTransform;
            _inputService = inputService;
            _artifactNetworkService = artifactNetworkService;
            _playerConfig = playerConfig;

            _inputService.OnInteractPressed += OnInteractPressed;
        }

        public void Dispose()
        {
            _inputService.OnInteractPressed -= OnInteractPressed;
        }

        private void OnInteractPressed()
        {
            if (_cameraTransform == null || _playerConfig == null)
            {
                return;
            }

            var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);

            if (!Physics.Raycast(ray, out var hit, _playerConfig.InteractionDistance))
            {
                return;
            }

            var collectable = hit.collider.GetComponent<ICollectable>();

            if (collectable == null || collectable.IsCollected)
            {
                return;
            }

            _artifactNetworkService.RequestCollect(collectable.Id);
        }
    }
}
