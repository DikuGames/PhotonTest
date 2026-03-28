using UnityEngine;

namespace Gameplay.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField]
        public GameObject PlayerPrefab { get; private set; }

        [field: SerializeField, Min(0f)]
        public float MoveSpeed { get; private set; } = 5f;

        [field: SerializeField, Min(0f)]
        public float LookSensitivity { get; private set; } = 180f;

        [field: SerializeField, Min(0f)]
        public float InteractionDistance { get; private set; } = 2f;

        [field: SerializeField]
        public float MinPitch { get; private set; } = -80f;

        [field: SerializeField]
        public float MaxPitch { get; private set; } = 80f;
    }
}
