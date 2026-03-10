using UnityEngine;

public interface IBullets
{
    public int Damage {get;set;}
    public float BulletSpeed {get;set;}

    protected void ApplyDamage(Character character);

    protected void BulletMovement();
}
