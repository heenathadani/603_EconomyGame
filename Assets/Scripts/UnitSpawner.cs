using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : UnitAbility
{
    public GameObject unitPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public override void Execute()
    {
        // Spawn the new worker and set it to Unit layer
        Vector3 spawnPt = transform.position;
        spawnPt.z -= GetComponent<MeshRenderer>().bounds.extents.z + 5;
        Instantiate(unitPrefab, spawnPt, Quaternion.identity);

        base.Execute();
    }
}