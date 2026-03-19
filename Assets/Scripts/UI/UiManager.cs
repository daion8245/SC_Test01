using System;
using Core;
using Parts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Player player;
    public static UiManager Instance;
    private DataManager _data;

    private int _stage;
    private int _score;
    private int _gold;

    [Header("기본 HUD")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("스테이지 타이머")]
    [SerializeField] private TextMeshProUGUI timerText;
    private float _elapsedTime;

    [Header("폭탄 UI")]
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private BombSystem bombSystem;

    [Header("퀵슬롯 UI")]
    [SerializeField] private TextMeshProUGUI slot1Text;
    [SerializeField] private TextMeshProUGUI slot2Text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player != null)
        {
            player.onHpChanged += RefreshHpBar;
            RefreshHpBar();
        }

        _data = DataManager.Instance;

        _stage = _data.stage;
        stageText.text = $"Stage :  {_stage}";
    }

    private void Update()
    {
        _gold = _data.gold;
        _score = _data.score;

        scoreText.text = _score.ToString();
        goldText.text = _gold.ToString();

        // 스테이지 타이머
        _elapsedTime += Time.deltaTime;
        if (timerText != null)
        {
            int minutes = (int)(_elapsedTime / 60);
            int seconds = (int)(_elapsedTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        // 폭탄 UI
        if (bombSystem != null && bombText != null)
            bombText.text = $"Bomb: {bombSystem.BombCount}";
    }

    private void RefreshHpBar()
    {
        if (player)
            hpBar.value = Mathf.InverseLerp(0f, player.MaxHp, player.Hp);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.onHpChanged -= RefreshHpBar;
    }
}
