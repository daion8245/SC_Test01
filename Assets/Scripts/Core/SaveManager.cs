using Core;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        var data = DataManager.Instance;
        if (data == null) return;

        PlayerPrefs.SetInt("Gold", data.gold);
        PlayerPrefs.SetInt("Stage", data.stage);
        PlayerPrefs.SetInt("Score", data.score);
        PlayerPrefs.Save();
        Debug.Log("게임 데이터 저장 완료!");
    }

    public void Load()
    {
        var data = DataManager.Instance;
        if (data == null) return;

        data.gold = PlayerPrefs.GetInt("Gold", 0);
        data.stage = PlayerPrefs.GetInt("Stage", 0);
        data.score = PlayerPrefs.GetInt("Score", 0);
        Debug.Log("게임 데이터 로드 완료!");
    }

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey("Gold");
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("게임 데이터 초기화 완료!");
    }
}
