using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DecoyBeacon : UnitAbility
{
    GameObject beaconPrefab;

    public DecoyBeacon()
    {
        abilityName = "Decoy Beacon";
        description = "Spawns a large, stationary decoy that draws enemies' attention.";
        cost = 300;
        cooldown = 200f;
    }

    public override bool Execute()
    {
        if (base.Execute())
        {
            beaconPrefab = GameObject.FindWithTag("Ew");

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 500f, 1 << 0) && beaconPrefab)
            {
                Instantiate(beaconPrefab, hit.point, Quaternion.identity);
            }
            else // didn't find a suitable place to spawn, so cancel and reset cooldown
            {
                timer = 0f;
            }
            return true;
        }
        return false;
    }
}
