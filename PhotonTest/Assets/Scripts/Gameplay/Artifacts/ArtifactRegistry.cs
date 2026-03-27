using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Artifacts
{
    public class ArtifactRegistry : MonoBehaviour
    {
        [SerializeField] private Artifact[] _artifacts;

        private readonly Dictionary<int, Artifact> _artifactsById = new();
        private IReadOnlyList<ICollectable> _collectables = new List<ICollectable>();

        public IReadOnlyList<ICollectable> Сollectables => _collectables;

        public int TotalCount => _artifactsById.Count;

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
            if (_artifactsById.TryGetValue(id, out var artifact))
            {
                collectable = artifact;
                return true;
            }

            collectable = null;
            return false;
        }

        private void BuildRegistry()
        {
            _artifactsById.Clear();

            if (_artifacts == null)
            {
                _collectables = new List<ICollectable>();
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
