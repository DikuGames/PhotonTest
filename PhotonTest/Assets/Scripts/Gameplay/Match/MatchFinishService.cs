using System;
using Gameplay.Artifacts;
using Zenject;

namespace Gameplay.Match
{
    public class MatchFinishService : IInitializable, IDisposable
    {
        private readonly ArtifactCollectionService _artifactCollectionService;

        private bool _isMatchFinishing;

        public event Action Finished;

        public MatchFinishService(ArtifactCollectionService artifactCollectionService)
        {
            _artifactCollectionService = artifactCollectionService;
        }

        public void Initialize()
        {
            _artifactCollectionService.ProgressChanged += OnProgressChanged;
        }

        public void Dispose()
        {
            _artifactCollectionService.ProgressChanged -= OnProgressChanged;
        }

        private void OnProgressChanged(int collectedCount, int totalCount)
        {
            if (_isMatchFinishing || totalCount == 0 || collectedCount < totalCount)
            {
                return;
            }

            _isMatchFinishing = true;
            Finished?.Invoke();
        }
    }
}
