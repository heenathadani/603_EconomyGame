using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalSpawner : MonoBehaviour
{
    public BiomassBank biomassBank;
    public int buildingCost = 100;
    public GameObject capitalBuildingPrefab;

    public void SpawnBuilding()
    {
        if (biomassBank.SpendBiomass(buildingCost))
        {
            Instantiate(capitalBuildingPrefab, transform.position, Quaternion.identity);
        }
    }
}
