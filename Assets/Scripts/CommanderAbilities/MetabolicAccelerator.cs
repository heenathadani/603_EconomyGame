using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetabolicAccelerator : UnitAbility
{
    public MetabolicAccelerator()
    {
        abilityName = "Metabolic Accelerator";
        description = "Double the Biomass collection speed of Workers for 15 seconds.";
        cost = 400;
        cooldown = 220f;
    }

    public override bool Execute()
    {
        if (base.Execute())
        {
            StartCoroutine(SetBiomassCollectionRate());

            return true;
        }
        return false;
    }

    IEnumerator SetBiomassCollectionRate()
    {
        foreach (Unit u in SelectionManager.allOtherUnits[UnitType.None])
        {
            if (u.TryGetComponent(out Biomass b))
            {
                b.collectionRate *= 2;
            }
        }

        yield return new WaitForSeconds(15);

        foreach (Unit u in SelectionManager.allOtherUnits[UnitType.None])
        {
            if (u.TryGetComponent(out Biomass b))
            {
                b.collectionRate /= 2;
            }
        }
    }
}
