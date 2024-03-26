using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomassBank : MonoBehaviour
{
    [SerializeField]
    private float biomass = 100;
    public float Biomass
    {
        get { return biomass; }
        private set 
        {
            UpdateBiomassText(value);
            biomass = value; 
        }
    }
    public TMPro.TextMeshProUGUI biomassText;

    private void Start()
    {
        UpdateBiomassText(Biomass);
    }

    public void AddBiomass(float amount)
    {
        Biomass += amount;
    }

    public bool SpendBiomass(float amount)
    {
        if (Biomass >= amount)
        {
            Biomass -= amount;
            return true;
        }
        return false;
    }

    public void UpdateBiomassText(float amount) // Update this method
    {
        biomassText.text = "Biomass: " + amount;
    }
}
