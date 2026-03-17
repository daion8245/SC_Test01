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

    private bool _loop = true;
    
    private IEnumerator ShellingStart()
    {
        FollowingPlayer();
        yield return new WaitForSeconds(inductiveTime);

        for (int i = 0; i <= _charactersInTrigger.Count; i++)
        {
            foreach (var character in _charactersInTrigger)
            {
                character.Hp -= Damage;
            }
        }
    }

    private void FollowingPlayer()
    {
        while (_loop)
        {
            transform.position = GameManager.Instance.playerPosition;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
            _charactersInTrigger.Add(character);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
            _charactersInTrigger.Remove(character);
    }

    public void ApplyDamage(Character character)
    {
        character.Hp -= Damage;
    }
}
