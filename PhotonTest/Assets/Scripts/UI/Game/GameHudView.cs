using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class GameHudView : MonoBehaviour
    {
        private const string ProgressFormat = "Collected: {0}/{1}";

        [SerializeField] private TMP_Text _progressText;

        public void SetProgress(int collectedCount, int totalCount)
        {
            _progressText.text = string.Format(ProgressFormat, collectedCount, totalCount);
        }
    }
}
