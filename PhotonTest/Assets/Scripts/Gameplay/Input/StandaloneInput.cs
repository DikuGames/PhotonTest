using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Input
{
    public class StandaloneInput : IInputService, ITickable
    {
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string MouseXAxis = "Mouse X";
        private const string MouseYAxis = "Mouse Y";
        private const int LeftMouseButton = 0;
        private const KeyCode InteractKey = KeyCode.E;
        
        public event Action OnDetectorActivated;
        public event Action OnInteractPressed;

        public Vector2 MoveInput => new Vector2(
            UnityEngine.Input.GetAxisRaw(HorizontalAxis),
            UnityEngine.Input.GetAxisRaw(VerticalAxis));
        
        public Vector2 LookInput => new Vector2(
            UnityEngine.Input.GetAxis(MouseXAxis),
            UnityEngine.Input.GetAxis(MouseYAxis));

        public void Tick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(LeftMouseButton))
            {
                OnDetectorActivated?.Invoke();
            }

            if (UnityEngine.Input.GetKeyDown(InteractKey))
            {
                OnInteractPressed?.Invoke();
            }
        }
    }
}
