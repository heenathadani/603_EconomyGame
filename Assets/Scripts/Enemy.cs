using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected enum State
    {
        Wandering,
        TrackingCapital,
        TrackingUnit,
        Idle,
    }

    //[SerializeField] private UnitManager unitManager;
    GameObject capitalUnit;

    private float nearestUnitDistance;

    private State currentState;


    protected override void Start()
    {
        base.Start();

        currentState = State.TrackingCapital;

        //Get Proper references to units
        //unitManager = GameObject.FindWithTag("UnitManager").GetComponent<UnitManager>();
        capitalUnit = GameObject.FindWithTag("CapitalUnit");
        FollowCapital();

        OnIdleTargetLost += FollowCapital;

        //InvokeRepeating("FindNearestUnit", 0, 0.5f);
    }

    void FollowCapital()
    {
        if (capitalUnit)
            Follow(capitalUnit.GetComponent<Unit>());
    }

    //protected override void Update()
    //{       
    //    switch (currentState)
    //    {
    //        case State.Idle:
    //            followUnit = null;
    //            break;
    //        case State.TrackingCapital:
    //            if (capitalUnit)
    //            {
    //                Follow(capitalUnit.GetComponent<Unit>());
    //            }
    //            break;
    //        case State.TrackingUnit:
    //            if (followUnit)
    //            {
    //                Follow(followUnit);
    //            }               
    //            break;
    //        default:
    //            break;
    //    }
    //    base.Update();
    //}

    //private void FindNearestUnit()
    //{
    //    bool targetsFoundInRange = false;
    //    foreach (var pair in SelectionManager.allFriendlyUnits)
    //    {
    //        foreach (Unit u in pair.Value)
    //        {
    //            float tempDistance = FindDistance(u.transform.position);
    //            if (tempDistance <= visionRange)
    //            {
    //                targetsFoundInRange = true;
    //                if (State.TrackingCapital == currentState)
    //                {
    //                    followUnit = u;
    //                    currentState = State.TrackingUnit;
    //                }
    //                else if (State.TrackingUnit == currentState)
    //                {
    //                    if (followUnit)
    //                    {
    //                        if (tempDistance < FindDistance(followUnit.transform.position))
    //                        {
    //                            followUnit = u;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        followUnit = u;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    if (!targetsFoundInRange)
    //    {
    //        currentState = State.TrackingCapital;
    //        if (capitalUnit)
    //            followUnit = capitalUnit.GetComponent<Unit>();
    //    }
    //}

 /*   protected float FindDistance(Vector2 targetLocation)
    {
        return Vector2.Distance(transform.position, targetLocation);
    }*/
}
