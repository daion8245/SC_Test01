using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageState
{
    Ready,
    Playing,
    Boss,
    Cleared,
    GameOver
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("스테이지 데이터 (1~3)")]
    [SerializeField] private StageData[] stageDatas;

    [Header("참조")]
    [SerializeField] private SpawnManager spawnManager;

    // 상태
    public StageState CurrentState { get; private set; }
    public int CurrentStageIndex => DataManager.Instance.stage;

    // 이벤트 (UI 연동용)
    public Action<StageState> onStateChanged;
    public Action onStageClear;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitStage();
    }

    private void InitStage()
    {
        int index = CurrentStageIndex;
        if (index >= stageDatas.Length)
        {
            SceneManager.LoadScene("Ending");
            return;
        }

        // SpawnManager에 현재 스테이지 데이터 할당
        spawnManager.stageData = stageDatas[index];

        // killCount 초기화
        if (DataManager.Instance != null)
            DataManager.Instance.killCount = 0;

        // 카운트다운 시작
        StartCoroutine(ReadyCountdown());
    }

    private IEnumerator ReadyCountdown()
    {
        SetState(StageState.Ready);

        // 3초 카운트다운
        yield return new WaitForSeconds(3f);

        // 플레이 시작
        SetState(StageState.Playing);
        spawnManager.StartSpawning();
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case StageState.Playing:
                UpdatePlaying();
                break;
            case StageState.Boss:
                UpdateBoss();
                break;
        }
    }

    private void UpdatePlaying()
    {
        // SpawnManager가 보스를 스폰했으면 Boss 상태로 전환
        if (spawnManager.BossSpawned)
        {
            SetState(StageState.Boss);
        }
    }

    private void UpdateBoss()
    {
        // 보스 포함 모든 적이 죽었으면 클리어
        if (GameManager.Instance.enemies.Count == 0)
        {
            OnStageClear();
        }
    }

    private void OnStageClear()
    {
        SetState(StageState.Cleared);

        var data = DataManager.Instance;

        // 보너스 점수/돈 지급
        data.score += data.killCount * 50;
        data.gold += data.killCount * 5;

        // 스테이지 진행
        data.stage++;

        // 이벤트 발행
        onStageClear?.Invoke();

        // 다음 씬 전환
        if (data.stage < stageDatas.Length)
        {
            SceneManager.LoadScene("Shop");
        }
        else
        {
            SceneManager.LoadScene("Ending");
        }
    }

    public void OnGameOver()
    {
        if (CurrentState == StageState.GameOver) return;

        SetState(StageState.GameOver);
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // 게임오버 UI 표시 대기
        yield return new WaitForSeconds(2f);

        // 현재 스테이지 재시작 (DataManager는 DontDestroyOnLoad이므로 돈/파츠 유지)
        SceneManager.LoadScene("Stage");
    }

    private void SetState(StageState newState)
    {
        CurrentState = newState;
        onStateChanged?.Invoke(newState);
    }
}
