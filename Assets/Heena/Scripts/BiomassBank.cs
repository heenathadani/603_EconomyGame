using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomassBank : MonoBehaviour
{
    public int biomass = 0;
    public TMPro.TextMeshProUGUI biomassText;

    private void Start()
    {
        biomassText.text = "Biomass: " + biomass;
    }

    public void AddBiomass(int amount)
    {
        biomass += amount;
        UpdateBiomassText();
    }

    public bool SpendBiomass(int amount)
    {
        if (biomass >= amount)
        {
            biomass -= amount;
            UpdateBiomassText();
            return true;
        }
        else
        {
            return false;
        }
    }
    private void UpdateBiomassText()
    {
        biomassText.text = "Biomass: " + biomass;
    }
}
