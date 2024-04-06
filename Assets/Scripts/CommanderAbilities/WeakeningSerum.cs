using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakeningSerum : UnitAbility
{
    // Start is called before the first frame update
    public WeakeningSerum()
    {
        abilityName = "Weakening Serum";
        description = "Reduces the HP of ALL enemy units to 1.";
        cooldown = 240f;
        cost = 500;
    }

    public override bool Execute()
    {
        if (base.Execute())
        {
            foreach (var pair in SelectionManager.allOtherUnits)
            {
                foreach (Unit u in pair.Value)
                {
                    if (u.Hostility == Hostility.Hostile)
                    {
                        u.SetCurrentHP(1);
                    }
                }
            }
            return true;
        }
        return false;
    }
}
