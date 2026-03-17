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

    private void Update()
    {
        if (player != null)
            hpBar.value = Mathf.InverseLerp(0f, player.MaxHp, player.Hp);
    }

    public void UpdateHp()
    {
        if (player != null)
            hpBar.value = Mathf.InverseLerp(0f, player.MaxHp, player.Hp);
    }
}
