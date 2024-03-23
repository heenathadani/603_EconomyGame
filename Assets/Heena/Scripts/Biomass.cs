using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Biomass : MonoBehaviour
{
    public int biomassAmount = 100;
    public void CollectResource(Unit worker)
    {
        worker.CollectBiomass(biomassAmount); 
        Destroy(gameObject);
    }
}