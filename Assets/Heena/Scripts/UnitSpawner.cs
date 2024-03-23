using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public BiomassBank biomassBank;
    public int workerCost = 50;
    public GameObject unitWorkerPrefab;
    public Transform unitsParent;

    private void Start()
    {
        unitsParent = GameObject.Find("Units").transform;
    }
    public void SpawnWorker()
    {
        if (biomassBank.SpendBiomass(workerCost))
        {
            // Get the layer index associated with the name "Unit"
            int unitLayer = LayerMask.NameToLayer("Unit");

            // Find all existing units within a certain radius
            Collider[] units = Physics.OverlapSphere(unitsParent.transform.position, 100f, 1 << unitLayer);

            if (units.Length > 0)
            {
                // If there are any existing units, spawn the new worker at a random position near the first one
                Vector3 spawnPosition = units[Random.Range(0, units.Length)].transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                GameObject newWorker = Instantiate(unitWorkerPrefab, spawnPosition, Quaternion.identity, unitsParent);

                // Set the layer of the new worker to "Unit"
                newWorker.layer = unitLayer;
            }
            else
            {
                // If there are no existing units, spawn the new worker at the default position
                GameObject newWorker = Instantiate(unitWorkerPrefab, unitsParent.transform.position, Quaternion.identity, unitsParent);

                // Set the layer of the new worker to "Unit"
                newWorker.layer = unitLayer;
            }
        }
    }
}
