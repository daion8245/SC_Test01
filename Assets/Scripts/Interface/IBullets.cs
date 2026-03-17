using UnityEngine;

public interface IBullets
{
    public int Damage {get;set;}
    public float BulletSpeed {get;set;}

    public void ApplyDamage(Character character);
}
