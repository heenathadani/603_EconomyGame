using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : UnitAbility
{
    Vector3 rallyPt;
    Unit unitRallyPt;

    // Start is called before the first frame update
    void Start()
    {
        rallyPt = transform.position;
        rallyPt.z -= 5f;
        abilityName = "Set Rally Point";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Execute()
    {
        Vector3 m = Input.mousePosition;
        m.z = Camera.main.nearClipPlane;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(m) - Camera.main.transform.position, out RaycastHit hit))
        {
            unitRallyPt = null;
            hit.collider.TryGetComponent(out unitRallyPt);
            if (!unitRallyPt)
                rallyPt = hit.point;
        }
    }

    public void SetRallyPoint(Vector3 position)
    {
        rallyPt = position;
    }
    public void SetRallyPoint(Unit unit)
    {
        unitRallyPt = unit;
    }
    public Vector3 GetRallyPoint()
    {
        return rallyPt;
    }
    public Unit GetRallyUnit()
    {
        return unitRallyPt;
    }
}
