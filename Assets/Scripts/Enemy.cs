using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    GameObject capitalUnit;
    public GameObject rnaPrefab;

    protected override void Start()
    {
        base.Start();

        capitalUnit = GameObject.FindWithTag("CapitalUnit");
        FollowCapital();

        OnIdleTargetLost += FollowCapital;
        OnKilled += (Unit u) =>
        {
            // 10% chance to drop RNA sample
            if (Random.Range(0, 100) < 10)
            {
                if (rnaPrefab)
                    Instantiate(rnaPrefab, transform.position, Quaternion.identity);
            }
        };
    }

    void FollowCapital()
    {
        if (capitalUnit)
            Follow(capitalUnit.GetComponent<Unit>());
    }
}
