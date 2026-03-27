using System;

namespace Gameplay.Artifacts
{
    public interface ICollectable
    {
        int Id { get; }
        bool IsCollected { get; }

        event Action<ICollectable> Collected;

        void Collect();
    }
}
