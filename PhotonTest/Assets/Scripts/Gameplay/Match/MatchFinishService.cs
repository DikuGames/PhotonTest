using System;
using Gameplay.Artifacts;
using Networking.Room;
using Zenject;

namespace Gameplay.Match
{
    public class MatchFinishService : IInitializable, IDisposable
    {
        private readonly ArtifactCollectionService _artifactCollectionService;
        private readonly IRoomExitService _roomExitService;

        private bool _isMatchFinishing;

        public MatchFinishService(ArtifactCollectionService artifactCollectionService, IRoomExitService roomExitService)
        {
            _artifactCollectionService = artifactCollectionService;
            _roomExitService = roomExitService;
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
            _roomExitService.ExitToLobby();
        }
    }
}
