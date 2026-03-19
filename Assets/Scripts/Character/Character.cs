using System;
using UnityEngine;
using Object = UnityEngine.Object;

public enum CharacterType
{
    Player,
    Enemy
}

public class Character : MonoBehaviour
{
    [SerializeField] protected new string name;
    [SerializeField] protected int hp = 100;
    [SerializeField] protected int maxHp = 100;
    [SerializeField] protected int atk = 10;
    [SerializeField] protected int def = 0;
    [SerializeField] protected bool isAlive = true;
    [SerializeField] protected CharacterType characterType = CharacterType.Player;

    public Action onHpChanged;

    public string Name { get => name; set => name = value; }
    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            onHpChanged?.Invoke();
            if (hp <= 0 && isAlive)
            {
                isAlive = false;
                Dead();
            }
        }
    }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int Atk { get => atk; set => atk = value; }
    public int Def { get => def; set => def = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public CharacterType Type { get => characterType; set => characterType = value;}

    protected virtual bool CrashTest(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Character _))
        {
            if (other.gameObject.GetComponentInChildren<Rigidbody>() != null)
            {
                CrashEntity(other);
                return true;
            }
        }

        else if (other.gameObject.TryGetComponent(out IBullets bullet))
        {
            CrashBullets(bullet);
            return true;
        }

        return false;
    }

    protected Character Chara;
    [SerializeField] private GameObject deadEffectPrefab;
    [SerializeField] private float deadEffectDisTime;
    [SerializeField] private float effectScale = 2f;

    protected virtual void CrashEntity(Collision other)
    {
        Chara = other.gameObject.GetComponentInParent<Character>();
        if (Chara == null) return;
        Debug.Log($"{Name}! {Chara.name} 충돌!");
    }

    public virtual void TakeDamage(int damage)
    {
        // 치트: 무적 모드 (플레이어만)
        if (CheatManager.isInvincible && characterType == CharacterType.Player)
            return;

        int actualDamage = Mathf.Max(damage - def, 1);
        Hp -= actualDamage;
    }

    protected virtual void CrashBullets(IBullets bullets)
    {
        Debug.Log($"{Name} 탄막 피격! {bullets.Damage} 데미지!");
        TakeDamage(bullets.Damage);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Dead()
    {
        Debug.Log($"{Name} 사망");
        EffectGeneration(GetComponent<Collider>());
        Destroy(gameObject);
    }
    
    protected void EffectGeneration(Collider collision)
    {
        if (deadEffectPrefab == null)
            return;
        
        Vector3 explosionPosition = transform.position;
        GameObject effect = Instantiate(deadEffectPrefab, explosionPosition, Quaternion.identity);
        effect.transform.localScale = new Vector3(effectScale, effectScale, effectScale);
        Destroy(effect, deadEffectDisTime);
        Destroy(gameObject);
    }

}
