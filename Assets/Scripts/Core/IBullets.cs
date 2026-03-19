using UnityEngine;

public interface IBullets
{
    public int Damage {get;set;}
    public float BulletSpeed {get;set;}

    public void ApplyDamage(Character character);

    public Vector3 LookPosition {get;set;}

    public void ForcedInduction();
}
