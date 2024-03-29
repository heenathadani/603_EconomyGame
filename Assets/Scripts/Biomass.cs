using System.Collections;
using UnityEngine;

public class Biomass : Unit
{
    public delegate void OnBeginExtractHandler();
    public event OnBeginExtractHandler OnBeginExtract;

    public float biomassAmount = 100;
    public float depletionTime = 10f;// Time in seconds to deplete the biomass

    bool extracting = false;

    public void OnTriggerEnter(Collider other)
    {
        if (extracting) return;
        
        if (other.TryGetComponent(out Unit unit) && unit.Hostility == Hostility.Friendly && unit.unitType == UnitType.WorkerUnit)
        {
            unit.Hostility = Hostility.Neutral;
            unit.Destroy();
            StartDepleting(unit);
            extracting = true;
            OnBeginExtract?.Invoke();
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
            float amount = 10; // Amount to deplete each second
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
       GameObject.FindGameObjectWithTag("BiomassBank").GetComponent<BiomassBank>().AddBiomass(amount);
    }
}