using UnityEngine;

namespace Gameplay.Player.Factory
{
    public interface IPlayerFactory
    {
        GameObject Create(Vector3 position, Quaternion rotation);
    }
}
