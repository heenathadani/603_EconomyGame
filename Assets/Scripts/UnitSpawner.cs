using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : UnitAbility
{
    public GameObject unitPrefab;

    public override bool Execute()
    {
        if (base.Execute())
        {
            // Spawn the new worker and set it to Unit layer
            Vector3 spawnPt = transform.position;
            spawnPt.z -= GetComponent<MeshRenderer>().bounds.extents.z + 5;
            Instantiate(unitPrefab, spawnPt, Quaternion.identity);
            return true;
        }
        return false;
    }
}
