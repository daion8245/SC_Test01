using Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 엔딩 씬 관리자
    /// 모든 스테이지 클리어 후 최종 점수 표시 및 타이틀 복귀
    /// </summary>
    public class EndingManager : MonoBehaviour
    {
        [Header("결과 표시")]
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI finalGoldText;
        [SerializeField] private TextMeshProUGUI clearMessageText;

        [Header("랭킹 등록")]
        [SerializeField] private GameObject rankingInputPanel;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button registerRankingButton;

        [Header("버튼")]
        [SerializeField] private Button titleButton;

        private const int MaxRankings = 5;
        private const string RankingCountKey = "RankingCount";
        private const string RankingNameKey = "Ranking_Name_";
        private const string RankingScoreKey = "Ranking_Score_";

        private void Start()
        {
            var data = DataManager.Instance;

            if (finalScoreText != null)
                finalScoreText.text = $"최종 점수: {data?.score ?? 0}";

            if (finalGoldText != null)
                finalGoldText.text = $"획득 골드: {data?.gold ?? 0} G";

            if (clearMessageText != null)
                clearMessageText.text = "전 스테이지 클리어!";

            if (titleButton != null)
                titleButton.onClick.AddListener(GoToTitle);

            if (registerRankingButton != null)
                registerRankingButton.onClick.AddListener(RegisterRanking);

            // 랭킹 등록 패널 표시
            if (rankingInputPanel != null)
                rankingInputPanel.SetActive(true);
        }

        private void RegisterRanking()
        {
            var data = DataManager.Instance;
            if (data == null) return;

            string playerName = (nameInputField != null && !string.IsNullOrEmpty(nameInputField.text))
                ? nameInputField.text
                : "PLAYER";

            int newScore = data.score;

            // 기존 랭킹 불러오기
            int count = PlayerPrefs.GetInt(RankingCountKey, 0);
            var names = new string[count];
            var scores = new int[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = PlayerPrefs.GetString(RankingNameKey + i, "???");
                scores[i] = PlayerPrefs.GetInt(RankingScoreKey + i, 0);
            }

            // 삽입 정렬로 순위 추가
            int insertAt = count;
            for (int i = 0; i < count; i++)
            {
                if (newScore > scores[i])
                {
                    insertAt = i;
                    break;
                }
            }

            int newCount = Mathf.Min(count + 1, MaxRankings);
            var newNames = new string[newCount];
            var newScores = new int[newCount];
            for (int i = 0; i < newCount; i++)
            {
                if (i == insertAt)
                {
                    newNames[i] = playerName;
                    newScores[i] = newScore;
                }
                else
                {
                    int srcIdx = i < insertAt ? i : i - 1;
                    if (srcIdx < count)
                    {
                        newNames[i] = names[srcIdx];
                        newScores[i] = scores[srcIdx];
                    }
                }
            }

            PlayerPrefs.SetInt(RankingCountKey, newCount);
            for (int i = 0; i < newCount; i++)
            {
                PlayerPrefs.SetString(RankingNameKey + i, newNames[i]);
                PlayerPrefs.SetInt(RankingScoreKey + i, newScores[i]);
            }
            PlayerPrefs.Save();

            Debug.Log($"[엔딩] 랭킹 등록 완료: {playerName} - {newScore}점");

            if (rankingInputPanel != null)
                rankingInputPanel.SetActive(false);
        }

        private void GoToTitle()
        {
            // 데이터 초기화 후 타이틀로 복귀
            var data = DataManager.Instance;
            if (data != null)
            {
                data.stage = 0;
                data.score = 0;
                data.gold = 0;
                data.killCount = 0;
                data.ResetSlots();
            }

            SceneManager.LoadScene("Title");
        }
    }
}
