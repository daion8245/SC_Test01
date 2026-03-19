using UnityEngine;

public class BombSystem : MonoBehaviour
{
    [SerializeField] private int bombCount = 3;
    public int BombCount => bombCount;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && bombCount > 0)
        {
            DestroyAllBullets();
            bombCount--;
            Debug.Log($"폭탄 사용! 남은 폭탄: {bombCount}");
        }
    }

    private void DestroyAllBullets()
    {
        if (GameManager.Instance == null) return;

        foreach (var bullet in GameManager.Instance.bullets.ToArray())
        {
            if (bullet is MonoBehaviour mb && mb != null)
                Destroy(mb.gameObject);
        }
        GameManager.Instance.bullets.Clear();
    }
}
