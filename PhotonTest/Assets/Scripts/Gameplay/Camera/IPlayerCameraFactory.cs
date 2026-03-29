using UnityEngine;

namespace Gameplay.Camera
{
    public interface IPlayerCameraFactory
    {
        GameObject Create(Transform parent);
    }
}
