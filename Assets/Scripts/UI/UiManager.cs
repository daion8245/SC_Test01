using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Player player;
    public static UiManager Instance;

    [SerializeField] private Slider hpBar;

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
