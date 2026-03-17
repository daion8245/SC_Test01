using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public Vector3 playerPosition;
    public Player player;
    public List<IEnemies> enemies = new List<IEnemies>();
    public List<IBullets> bullets = new List<IBullets>();

    public int score;
    public int money;
    public int currentStage = 1;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Screen.fullScreen = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}