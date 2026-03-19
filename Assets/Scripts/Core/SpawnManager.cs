using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("현재 스테이지 데이터")]
    public StageData stageData;

    [Header("필드 크기 (실 사용 영역의 절반)")]
    [SerializeField] private float fieldHalfWidth = 200f;
    [SerializeField] private float fieldHalfHeight = 107.5f;

    [Header("스폰 마진 (필드 밖 오프셋)")]
    public float spawnMargin = 5f;

    private float elapsedTime;
    private int spawnIndex;
    private bool bossSpawned;
    private bool isSpawning;

    public bool IsAllSpawned => stageData != null && spawnIndex >= stageData.spawns.Length;
    public bool BossSpawned => bossSpawned;

    void Start()
    {
        elapsedTime = 0f;
        spawnIndex = 0;
        bossSpawned = false;
        isSpawning = false;
    }

    public void StartSpawning()
    {
        elapsedTime = 0f;
        spawnIndex = 0;
        bossSpawned = false;
        isSpawning = true;
    }

    void Update()
    {
        if (!isSpawning) return;

        elapsedTime += Time.deltaTime;

        // 일반 적 스폰 체크
        while (spawnIndex < stageData.spawns.Length
            && elapsedTime >= stageData.spawns[spawnIndex].time)
        {
            SpawnEntry entry = stageData.spawns[spawnIndex];
            for (int i = 0; i < entry.count; i++)
                SpawnEnemy(entry.enemyPrefab);
            spawnIndex++;
        }

        // 보스 스폰
        if (!bossSpawned && elapsedTime >= stageData.bossSpawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        Vector3 pos = GetRandomEdgePosition();
        Instantiate(prefab, pos, Quaternion.identity);
    }

    void SpawnBoss()
    {
        // 보스는 상단 중앙(Z+)에서 등장
        Vector3 top = new Vector3(0f, 0f, fieldHalfHeight + spawnMargin);
        Instantiate(stageData.bossPrefab, top, Quaternion.identity);
    }

    // 화면 4면 중 랜덤 한 면의 중앙에서 스폰 위치 계산
    Vector3 GetRandomEdgePosition()
    {
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: return new Vector3(0f, 0f, fieldHalfHeight + spawnMargin);   // 위 (Z+)
            case 1: return new Vector3(0f, 0f, -fieldHalfHeight - spawnMargin);  // 아래 (Z-)
            case 2: return new Vector3(-fieldHalfWidth - spawnMargin, 0f, 0f);   // 왼쪽 (X-)
            case 3: return new Vector3(fieldHalfWidth + spawnMargin, 0f, 0f);    // 오른쪽 (X+)
            default: return Vector3.zero;
        }
    }
}
