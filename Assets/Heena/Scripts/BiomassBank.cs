using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomassBank : MonoBehaviour
{
    [SerializeField]
    private int biomass = 100;
    public int Biomass
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

    public void AddBiomass(int amount)
    {
        Biomass += amount;
    }

    public bool SpendBiomass(int amount)
    {
        if (Biomass >= amount)
        {
            Biomass -= amount;
            return true;
        }
        return false;
    }

    public void UpdateBiomassText(int amount) // Update this method
    {
        biomassText.text = "Biomass: " + amount;
    }
}
