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
    [SerializeField] private GameObject capitalUnit;
    public float visionRange;

    private GameObject targetUnit;
    private float nearestUnitDistance;

    private State currentState;


    protected override void Start()
    {
        hostility = Hostility.Hostile;
        currentState = State.TrackingCapital;

        //Get Proper references to units
        //unitManager = GameObject.FindWithTag("UnitManager").GetComponent<UnitManager>();
        capitalUnit = GameObject.FindWithTag("CapitalUnit");


        base.Start();

        InvokeRepeating("FindNearestUnit", 0, 0.5f);
    }

    protected override void Update()
    {       
        switch (currentState)
        {
            case State.Idle:
                followUnit = null;
                break;
            case State.TrackingCapital:
                if (capitalUnit)
                {
                    Follow(capitalUnit.GetComponent<Unit>());
                }
                break;
            case State.TrackingUnit:
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

    private void FindNearestUnit()
    {
        unitManager.UpdatePlayerUnitsList();
        bool targetsFoundInRange = false;
        foreach(GameObject g in unitManager.playerUnits)
        {

            float tempDistance = FindDistance(g.transform.position);
            if(tempDistance <= visionRange)
            {
                targetsFoundInRange = true;
                if (State.TrackingCapital == currentState)
                {
                    targetUnit = g;
                    followUnit = targetUnit.GetComponent<Unit>();
                    currentState = State.TrackingUnit;
                }
                else if(State.TrackingUnit == currentState)
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
            currentState = State.TrackingCapital;
            followUnit = capitalUnit.GetComponent<Unit>();
        }
    }

 /*   protected float FindDistance(Vector2 targetLocation)
    {
        return Vector2.Distance(transform.position, targetLocation);
    }*/
}
