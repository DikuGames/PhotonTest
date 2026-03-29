using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Artifacts
{
    public class ArtifactRegistry : MonoBehaviour
    {
        [SerializeField] private Artifact[] _artifacts;

        private readonly Dictionary<int, Artifact> _artifactsById = new();
        private IReadOnlyList<ICollectable> _collectables = new List<ICollectable>();
        private bool _isInitialized;

        public IReadOnlyList<ICollectable> Artifacts
        {
            get
            {
                EnsureInitialized();
                return _collectables;
            }
        }

        public int TotalCount
        {
            get
            {
                EnsureInitialized();
                return _artifactsById.Count;
            }
        }

        private void Awake()
        {
            BuildRegistry();
        }

        private void OnValidate()
        {
            ValidateDuplicateIds();
        }

        public bool TryGet(int id, out ICollectable collectable)
        {
            EnsureInitialized();

            if (_artifactsById.TryGetValue(id, out var artifact))
            {
                collectable = artifact;
                return true;
            }

            collectable = null;
            return false;
        }

        private void EnsureInitialized()
        {
            if (_isInitialized)
            {
                return;
            }

            BuildRegistry();
        }

        private void BuildRegistry()
        {
            _artifactsById.Clear();

            if (_artifacts == null)
            {
                _collectables = new List<ICollectable>();
                _isInitialized = true;
                return;
            }

            var collectables = new List<ICollectable>(_artifacts.Length);

            foreach (var artifact in _artifacts)
            {
                if (artifact == null)
                {
                    continue;
                }

                if (_artifactsById.ContainsKey(artifact.Id))
                {
                    Debug.LogError($"Duplicate artifact id detected: {artifact.Id}", artifact);
                    continue;
                }

                _artifactsById.Add(artifact.Id, artifact);
                collectables.Add(artifact);
            }

            _collectables = collectables;
            _isInitialized = true;
        }

        private void ValidateDuplicateIds()
        {
            if (_artifacts == null)
            {
                return;
            }

            var ids = new HashSet<int>();

            foreach (var artifact in _artifacts)
            {
                if (artifact == null)
                {
                    continue;
                }

                if (!ids.Add(artifact.Id))
                {
                    Debug.LogError($"Duplicate artifact id detected: {artifact.Id}", artifact);
                }
            }
        }
    }
}
