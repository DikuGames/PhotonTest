using Gameplay.Input;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerDetector : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        [SerializeField] private Renderer _detector;

        private MaterialPropertyBlock _propertyBlock;
        private IInputService _inputService;
        private PlayerConfig _playerConfig;
        private bool _isSubscribedToInput;
        private bool _isInitialized;
        private bool _isActive;

        public bool IsActive => _isActive;

        public void Initialize(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            _propertyBlock ??= new MaterialPropertyBlock();
            _isInitialized = true;
            UpdateColor();
        }

        public void BindInput(IInputService inputService)
        {
            if (_isSubscribedToInput)
            {
                UnbindInput();
            }

            _inputService = inputService;
            _inputService.OnDetectorActivated += Toggle;
            _isSubscribedToInput = true;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;

            if (_isInitialized)
            {
                UpdateColor();
            }
        }

        public void Dispose()
        {
            UnbindInput();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Toggle()
        {
            SetActive(!_isActive);
        }

        private void UnbindInput()
        {
            if (!_isSubscribedToInput || _inputService == null)
            {
                return;
            }

            _inputService.OnDetectorActivated -= Toggle;
            _inputService = null;
            _isSubscribedToInput = false;
        }

        private void UpdateColor()
        {
            var targetColor = _isActive
                ? _playerConfig.DetectorActiveColor
                : _playerConfig.DetectorInactiveColor;

            _detector.GetPropertyBlock(_propertyBlock);

            if (_detector.sharedMaterial != null && _detector.sharedMaterial.HasProperty(BaseColorId))
            {
                _propertyBlock.SetColor(BaseColorId, targetColor);
            }
            else
            {
                _propertyBlock.SetColor(ColorId, targetColor);
            }

            _detector.SetPropertyBlock(_propertyBlock);
        }
    }
}
