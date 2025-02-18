using System.Collections;
using _ImmersiveGames.Scripts.GameSystems;
using TMPro;
using UnityEngine;

namespace _ImmersiveGames.Scripts.UI {
    public class ScoreUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start() {
            UpdateScore();
        }

        public void UpdateScore() {
            StartCoroutine(UpdateScoreNextFrame());
        }

        private IEnumerator UpdateScoreNextFrame() {
            // Make sure all logic has run before updating the score
            yield return null;
            scoreText.text = GameManager.Instance.Score.ToString();
        }
    }
}