using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelling : MonoBehaviour, IBullets
{
    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float inductiveTime;

    private List<Character> _charactersInTrigger = new List<Character>();
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Damage = damage;
        BulletSpeed = bulletSpeed;
        StartCoroutine(ShellingStart());
    }
    
    private IEnumerator ShellingStart()
    {
        float elapsed = 0f;
        while (elapsed < inductiveTime)
        {
            _rigidbody.MovePosition(GameManager.Instance.playerPosition);
            elapsed += Time.fixedDeltaTime;
            // 물리 엔진 타이밍에 맞춰 실행
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(2f);
        foreach (var character in _charactersInTrigger)
        {
            character.Hp -= Damage;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("테스트02");
        if (other.TryGetComponent(out Character character))
            _charactersInTrigger.Add(character);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character))
            _charactersInTrigger.Remove(character);
    }

    public void ApplyDamage(Character character)
    {
        Debug.Log("테스트01");
    }
}
