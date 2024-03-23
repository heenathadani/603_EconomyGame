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
            int unitLayer = LayerMask.NameToLayer("Unit");
            Collider[] units = Physics.OverlapSphere(unitsParent.transform.position, 100f, 1 << unitLayer);

            if (units.Length > 0)
            {
                Vector3 spawnPosition = units[Random.Range(0, units.Length)].transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                GameObject newWorker = Instantiate(unitWorkerPrefab, spawnPosition, Quaternion.identity, unitsParent);
                newWorker.layer = unitLayer;
            }
            else
            {
                GameObject newWorker = Instantiate(unitWorkerPrefab, unitsParent.transform.position, Quaternion.identity, unitsParent);
                newWorker.layer = unitLayer;
            }
        }
    }
}
