using System;

namespace Gameplay.Artifacts
{
    public class ArtifactCollectionService : IDisposable
    {
        private readonly ArtifactRegistry _artifactRegistry;

        public event Action<int, int> ProgressChanged;

        public int CollectedCount { get; private set; }
        public int TotalCount => _artifactRegistry.TotalCount;

        public ArtifactCollectionService(ArtifactRegistry artifactRegistry)
        {
            _artifactRegistry = artifactRegistry;
            Subscribe();
            RefreshProgress();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void OnCollected(ICollectable collectable)
        {
            CollectedCount++;
            ProgressChanged?.Invoke(CollectedCount, TotalCount);
        }

        private void Subscribe()
        {
            foreach (var collectable in _artifactRegistry.Artifacts)
            {
                collectable.Collected += OnCollected;
            }
        }

        private void Unsubscribe()
        {
            foreach (var collectable in _artifactRegistry.Artifacts)
            {
                collectable.Collected -= OnCollected;
            }
        }

        private void RefreshProgress()
        {
            CollectedCount = 0;

            foreach (var collectable in _artifactRegistry.Artifacts)
            {
                if (collectable.IsCollected)
                {
                    CollectedCount++;
                }
            }

            ProgressChanged?.Invoke(CollectedCount, TotalCount);
        }
    }
}
