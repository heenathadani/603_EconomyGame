using System.Collections;
using UnityEngine;

public class Biomass : Unit
{
    public delegate void OnBeginExtractHandler();
    public event OnBeginExtractHandler OnBeginExtract;

    public float biomassAmount = 100;
    public float depletionTime = 10f;// Time in seconds to deplete the biomass
    public void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit && unit.unitType == UnitType.WorkerUnit)
        {
            unit.Destroy();
            StartDepleting(unit);
            OnBeginExtract?.Invoke();
        }
    }
    public void StartDepleting(Unit worker)
    {
        StartCoroutine(DepleteOverTime(worker));
    }
    //private IEnumerator DepleteOverTime(Unit worker)
    //{
    //    float rate = biomassAmount / depletionTime; // Amount to deplete per second

    //    while (biomassAmount > 0)
    //    {
    //        float amount = rate * Time.deltaTime; // Amount to deplete this frame
    //        biomassAmount -= amount;
    //        worker.CollectBiomass(amount);
    //        yield return null; // Wait for next frame
    //    }
    //    Destroy(gameObject);
    //}
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