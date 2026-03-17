using UnityEngine;

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
    [SerializeField] protected bool isAlive = true;
    [SerializeField] protected CharacterType characterType = CharacterType.Player;
    
    public string Name { get => name; set => name = value; }
    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            if (hp <= 0 && isAlive)
            {
                isAlive = false;
                Dead();
            }
        }
    }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int Atk { get => atk; set => atk = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public CharacterType Type { get => characterType; set => characterType = value;}

    protected bool CrashTest(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Character _))
        {
            CrashEntity(other);
            return true;
        }

        if (other.gameObject.TryGetComponent(out IBullets bullet))
        {
            CrashBullets(bullet);
            return true;
        }

        return false;
    }

    protected Character Chara;

    protected virtual void CrashEntity(Collision other)
    {
        Chara = other.gameObject.GetComponentInParent<Character>();
        if (Chara == null) return;
        Debug.Log($"{Name}! {Chara.name} 충돌!");
    }

    protected virtual void CrashBullets(IBullets bullets)
    {
        Debug.Log($"{Name} 탄막 피격! {bullets.Damage} 데미지!");
        Hp -= bullets.Damage;
    }

    public virtual void Dead()
    {
        Debug.Log($"{Name} 사망");
    }
}