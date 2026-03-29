using System;
using Gameplay.Artifacts;
using Zenject;

namespace UI.Game
{
    public class GameHud : IInitializable, IDisposable
    {
        private readonly GameHudView _view;
        private readonly ArtifactCollectionService _artifactCollectionService;

        public GameHud(GameHudView view, ArtifactCollectionService artifactCollectionService)
        {
            _view = view;
            _artifactCollectionService = artifactCollectionService;
        }

        public void Initialize()
        {
            _artifactCollectionService.ProgressChanged += OnProgressChanged;
            _view.SetProgress(_artifactCollectionService.CollectedCount, _artifactCollectionService.TotalCount);
        }

        public void Dispose()
        {
            _artifactCollectionService.ProgressChanged -= OnProgressChanged;
        }

        private void OnProgressChanged(int collectedCount, int totalCount)
        {
            _view.SetProgress(collectedCount, totalCount);
        }
    }
}
