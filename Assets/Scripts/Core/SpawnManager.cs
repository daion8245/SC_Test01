using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("현재 스테이지 데이터")]
    public StageData stageData;

    [Header("스폰 범위 (화면 가장자리)")]
    public float spawnMargin = 0.5f;

    private float elapsedTime;
    private int spawnIndex;
    private bool bossSpawned;
    private bool isSpawning;
    private Camera cam;

    public bool IsAllSpawned => stageData != null && spawnIndex >= stageData.spawns.Length;
    public bool BossSpawned => bossSpawned;

    void Start()
    {
        cam = Camera.main;
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
        Vector2 pos = GetRandomEdgePosition();
        Instantiate(prefab, pos, Quaternion.identity);
    }

    void SpawnBoss()
    {
        // 보스는 화면 상단 중앙에서 등장
        Vector2 top = cam.ViewportToWorldPoint(new Vector3(0.5f, 1.1f, 0f));
        Instantiate(stageData.bossPrefab, top, Quaternion.identity);
    }

    // 화면 4면 중 랜덤 한 면의 가장자리에서 스폰 위치 계산
    Vector2 GetRandomEdgePosition()
    {
        int side = Random.Range(0, 4);
        float vx = 0f, vy = 0f;

        switch (side)
        {
            case 0: vx = Random.value; vy = 1f + spawnMargin; break;  // 위
            case 1: vx = Random.value; vy = -spawnMargin; break;       // 아래
            case 2: vx = -spawnMargin; vy = Random.value; break;       // 왼쪽
            case 3: vx = 1f + spawnMargin; vy = Random.value; break;   // 오른쪽
        }

        Vector3 worldPos = cam.ViewportToWorldPoint(new Vector3(vx, vy, 0f));
        worldPos.z = 0f;
        return worldPos;
    }
}
