using System.Collections;
using UnityEngine;

public class TrsTEnemy : RsacEnemy
{
    [SerializeField] private float sequenceFireRate = 0.5f;
    
    protected override IEnumerator Fire()
    {
        loop = false;
        yield return new WaitForSeconds(fireRate);
        for (int i = 0; i < 3; i++)
        {
            FiringBullet();
            yield return new WaitForSeconds(sequenceFireRate);
        }
        loop = true;
    }
}
