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

    private void Start()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        StartCoroutine(ShellingStart());
    }

    private IEnumerator ShellingStart()
    {
        float elapsed = 0f;
        while (elapsed < inductiveTime)
        {
            transform.position = GameManager.Instance.playerPosition;
            elapsed += Time.deltaTime;
            yield return null;
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
        character.Hp -= Damage;
    }
}
