using UnityEngine;

// 스폰 1건의 정보 (몇 초에 어떤 적을 몇 마리)
[System.Serializable]
public class SpawnEntry
{
    public float time;
    public GameObject enemyPrefab;
    public int count = 1;
}

// 스테이지 1개의 스폰 시간표
[CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public SpawnEntry[] spawns;
    public GameObject bossPrefab;
    public float bossSpawnTime = 60f;
}
