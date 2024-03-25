using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : UnitAbility
{
    public GameObject unitWorkerPrefab;

    Vector3 spawnPt;

    private void Start()
    {
        spawnPt = transform.position;
        spawnPt.z -= GetComponent<MeshRenderer>().bounds.extents.z + 1;
    }

    public override void Execute()
    {
        // Spawn the new worker and set it to Unit layer
        GameObject newWorker = Instantiate(unitWorkerPrefab, spawnPt, Quaternion.identity);
        newWorker.layer = LayerMask.NameToLayer("Unit");
    }
}
