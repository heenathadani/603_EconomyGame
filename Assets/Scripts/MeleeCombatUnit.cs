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

    public float visionRange;

    private float nearestUnitDistance;

    private State currentState;


    protected override void Start()
    {
        base.Start();
        currentState = State.TrackingEnemy;

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
                if (followUnit)
                {
                    Follow(followUnit);
                }
                break;
            default:
                break;
        }
        base.Update();
    }

    private void FindNearestEnemy()
    {
        bool targetsFoundInRange = false;
        foreach (var pair in SelectionManager.allOtherUnits)
        {
            foreach (Unit u in pair.Value)
            {
                if (u.hostility == Hostility.Hostile)
                {
                    float tempDistance = FindDistance(u.transform.position);
                    if (tempDistance <= visionRange)
                    {
                        targetsFoundInRange = true;
                        if (State.TrackingEnemy == currentState)
                        {
                            if (followUnit)
                            {
                                if (tempDistance < FindDistance(u.transform.position))
                                {
                                    followUnit = u;
                                }
                            }
                            else
                            {
                                followUnit = u;
                            }
                        }
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
