using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRegeneration : UnitAbility
{
    public CellRegeneration()
    {
        abilityName = "Cell Regeneration";
        description = "Fully heal all friendly units within 10m of your Capital, including the Capital itself.";
        cooldown = 45f;
        cost = 300;
    }

    public override bool Execute()
    {
        if (base.Execute())
        {
            GameObject g = GameObject.FindWithTag("CapitalUnit");
            if (g)
            {
                // Get all friendly units in a 10m radius and heal them to full
                Collider[] cols = Physics.OverlapSphere(g.transform.position, 10f, 1 << 6);
                foreach (Collider c in cols)
                {
                    Unit u = c.GetComponent<Unit>();
                    u.SetCurrentHP(u.maxHP);
                }
            }
            return true;
        }
        return false;
    }
}
