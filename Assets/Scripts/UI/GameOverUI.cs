using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 게임오버 UI 처리
    /// StageManager의 상태 변화를 구독하여 게임오버 패널 표시
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [Header("게임오버 패널")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            if (StageManager.Instance != null)
                StageManager.Instance.onStateChanged += OnStateChanged;
        }

        private void OnStateChanged(StageState state)
        {
            if (state == StageState.GameOver)
                ShowGameOver();
        }

        private void ShowGameOver()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            if (gameOverText != null)
                gameOverText.text = "GAME OVER";

            if (scoreText != null && Core.DataManager.Instance != null)
                scoreText.text = $"점수: {Core.DataManager.Instance.score}";
        }

        private void OnDestroy()
        {
            if (StageManager.Instance != null)
                StageManager.Instance.onStateChanged -= OnStateChanged;
        }
    }
}
