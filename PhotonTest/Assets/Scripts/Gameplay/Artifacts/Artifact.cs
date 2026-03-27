using System;
using UnityEngine;

namespace Gameplay.Artifacts
{
    public class Artifact : MonoBehaviour, ICollectable
    {
        [field: SerializeField]
        public int Id { get; private set; }

        public bool IsCollected { get; private set; }
        public event Action<ICollectable> Collected;

        public void Collect()
        {
            if (IsCollected)
            {
                return;
            }

            IsCollected = true;
            Collected?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
