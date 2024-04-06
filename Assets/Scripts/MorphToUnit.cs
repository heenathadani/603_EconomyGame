using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphToUnit : UnitAbility
{
    public GameObject unitToMorphTo;

    public override bool Execute()
    {
        if (base.Execute())
        {
            // replace this unit with the new unit to morph to
            Instantiate(unitToMorphTo, transform.position, transform.rotation);
            GetComponent<Unit>().Destroy();
            return true;
        }
        return false;
    }
}
