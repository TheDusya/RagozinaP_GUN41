using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class TextManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _bonusText;
        [SerializeField]
        private TextMeshProUGUI _currentScoreText;
        [SerializeField]
        private TextMeshProUGUI _totalScoreText;

        public void OnEnable()
        {
            _currentScoreText.text = "0";
            _totalScoreText.text = "0";
            _bonusText.text = "+0";
        }

        public void WriteCurrent(int points) => _currentScoreText.text = points.ToString();
        public void WriteTotal(int points) => _totalScoreText.text = points.ToString();
        public void WriteBonus(int points) => _bonusText.text = points.ToString();
    }
}
