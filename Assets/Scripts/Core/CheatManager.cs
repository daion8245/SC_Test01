using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private static CheatManager _instance;
    public static CheatManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
