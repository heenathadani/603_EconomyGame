using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatUnit : Unit
{
    protected enum State
    {
        TrackingEnemy,
        Idle,
    }

    [SerializeField] private EnemyManager enemyManager;
    public float visionRange;

    private GameObject targetUnit;
    private float nearestUnitDistance;

    private State currentState;


    protected override void Start()
    {
        hostility = Hostility.Friendly;
        currentState = State.TrackingEnemy;

        //Get Proper references to units
        enemyManager = GameObject.FindWithTag("EnemyManager").GetComponent<EnemyManager>();


        base.Start();

        InvokeRepeating("FindNearestEnemy", 0, 0.5f);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                followUnit = null;
                break;
            case State.TrackingEnemy:
                if (targetUnit)
                {
                    Follow(targetUnit.GetComponent<Unit>());
                }
                break;
            default:
                break;
        }
        base.Update();
    }

    private void FindNearestEnemy()
    {
        enemyManager.UpdateEnemyUnitsList();
        bool targetsFoundInRange = false;
        foreach (GameObject g in enemyManager.enemyUnits)
        {

            float tempDistance = FindDistance(g.transform.position);
            if (tempDistance <= visionRange)
            {
                targetsFoundInRange = true;
                if (State.TrackingEnemy == currentState)
                {
                    if (targetUnit)
                    {
                        if (tempDistance < FindDistance(targetUnit.transform.position))
                        {
                            targetUnit = g;
                            followUnit = targetUnit.GetComponent<Unit>();
                        }
                    }
                    else
                    {
                        targetUnit = g;
                        followUnit = targetUnit.GetComponent<Unit>();
                    }

                }
            }
        }

        if (!targetsFoundInRange)
        {
            currentState = State.Idle;
            followUnit = null;
        }
    }
}
