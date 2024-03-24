using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RallyPoint))]
public class UnitSpawner : UnitAbility
{
    public BiomassBank biomassBank;
    public GameObject unitWorkerPrefab;

    Vector3 spawnPt;

    private void Start()
    {
        spawnPt = transform.position;
        spawnPt.z -= 0.5f;
    }

    public override void Execute()
    {
        if (biomassBank.SpendBiomass(cost))
        {
            // Spawn the new worker and set it to Unit layer
            GameObject newWorker = Instantiate(unitWorkerPrefab, spawnPt, Quaternion.identity);
            newWorker.layer = LayerMask.NameToLayer("Unit");
            Unit rallyUnit = GetComponent<RallyPoint>().GetRallyUnit();
            if (rallyUnit)
                newWorker.GetComponent<Unit>().Follow(rallyUnit);
            else
                newWorker.GetComponent<Unit>().MoveTo(GetComponent<RallyPoint>().GetRallyPoint());
        }
    }
}
