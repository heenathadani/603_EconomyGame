using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biomass : Unit
{
    public delegate void OnBeginExtractHandler();
    public event OnBeginExtractHandler OnBeginExtract;

    public float biomassAmount = 100;
    public float collectionRate = 10f;

    int extracting = 0;

    BiomassBank bank;
    public List<GameObject> ViralInjector;
    protected override void Start()
    {
        base.Start();
        bank = GameObject.FindGameObjectWithTag("BiomassBank").GetComponent<BiomassBank>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (extracting==3)
        {
            return;
        }
        
        if (other.TryGetComponent(out Unit unit) && unit.Hostility == Hostility.Friendly && unit.unitType == UnitType.WorkerUnit)
        {
            Debug.Log("Conditions met");
            unit.Hostility = Hostility.Neutral;
            unit.Destroy();
            StartDepleting(unit);
            ViralInjector[extracting].SetActive(true);
            extracting ++;
            OnBeginExtract?.Invoke();
            Debug.Log(OnBeginExtract);
        }
    }
    public void StartDepleting(Unit worker)
    {
        StartCoroutine(DepleteOverTime(worker));
    }

    private IEnumerator DepleteOverTime(Unit worker)
    {
        while (biomassAmount > 0)
        {
            float amount = collectionRate; // Amount to deplete each second
            if (biomassAmount < amount)
            {
                amount = biomassAmount; // If less than 10 biomass remains, only deplete the remaining amount
            }
            biomassAmount -= amount;
            CollectBiomass(amount);
            yield return new WaitForSeconds(1); // Wait for 1 second
        }
        Destroy();
    }
    public void CollectBiomass(float amount)
    {
        if (bank)
        {
            bank.AddBiomass(amount);
        }
    }
}