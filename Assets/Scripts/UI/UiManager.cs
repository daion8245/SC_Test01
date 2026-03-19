using System;
using Core;
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

    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goldText;
    

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
