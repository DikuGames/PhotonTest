using System;
using UnityEngine;

namespace Gameplay.Input
{
    public interface IInputService
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }

        event Action OnDetectorActivated;
        event Action OnInteractPressed;
    }
}
