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
        private bool _isActive;

        public bool IsActive => _isActive;

        public void Initialize(IInputService inputService, PlayerConfig playerConfig)
        {
            _inputService = inputService;
            _playerConfig = playerConfig;
            _inputService.OnDetectorActivated += Toggle;
            
            _propertyBlock = new MaterialPropertyBlock();
            _detector.GetPropertyBlock(_propertyBlock);
            SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            UpdateColor();
        }

        public void Dispose()
        {
            _inputService.OnDetectorActivated -= Toggle;
            _inputService = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Toggle()
        {
            SetActive(!_isActive);
        }

        private void UpdateColor()
        {
            var targetColor = _isActive
                ? _playerConfig.DetectorActiveColor
                : _playerConfig.DetectorInactiveColor;

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
