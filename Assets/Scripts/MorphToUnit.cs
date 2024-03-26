using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphToUnit : UnitAbility
{
    public GameObject unitToMorphTo;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Execute()
    {
        // replace this unit with the new unit to morph to
        Instantiate(unitToMorphTo, transform.position, transform.rotation);
        GetComponent<Unit>().TakeDamage(1000);
    }
}
