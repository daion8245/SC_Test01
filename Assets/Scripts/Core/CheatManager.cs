using Core;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static bool isDebugMode = false;
    public static bool isInvincible = false;

    void Update()
    {
        // F1: 디버그 모드 (충돌 범위 표시)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isDebugMode = !isDebugMode;
            Debug.Log($"디버그 모드: {(isDebugMode ? "ON" : "OFF")}");
        }

        // F2: 무적 모드
        if (Input.GetKeyDown(KeyCode.F2))
        {
            isInvincible = !isInvincible;
            Debug.Log($"무적 모드: {(isInvincible ? "ON" : "OFF")}");
        }

        // F3: 강제 적 사망
        if (Input.GetKeyDown(KeyCode.F3))
        {
            KillAllEnemies();
            Debug.Log("치트: 모든 적 처치!");
        }

        // F4: 돈 추가
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.gold += 1000;
                Debug.Log($"치트: 돈 +1000 (현재: {DataManager.Instance.gold})");
            }
        }
    }

    void KillAllEnemies()
    {
        if (GameManager.Instance == null) return;

        foreach (var enemy in GameManager.Instance.enemies.ToArray())
        {
            if (enemy is EnemyBase eb)
                eb.TakeDamage(99999);
        }
    }

    // 디버그 모드 시각화: 모든 Collider 범위 표시
    void OnDrawGizmos()
    {
        if (!isDebugMode) return;

        Collider[] allColliders = FindObjectsByType<Collider>(FindObjectsSortMode.None);
        Gizmos.color = Color.green;

        foreach (var col in allColliders)
        {
            if (col is BoxCollider box)
            {
                Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
            }
            else if (col is SphereCollider sphere)
            {
                Gizmos.DrawWireSphere(sphere.bounds.center, sphere.radius * sphere.transform.lossyScale.x);
            }
            else if (col is CapsuleCollider capsule)
            {
                Gizmos.DrawWireSphere(capsule.bounds.center, capsule.bounds.extents.magnitude);
            }
        }
    }
}
