using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public RectTransform selectBox;

    List<Unit> newSelected, selected;

    // Sets automatically handle dupes
    public static HashSet<Unit> allFriendlyUnits, allOtherUnits;

    Vector2 mouseStart;
    Camera cam;

    private void Awake()
    {
        newSelected = new();
        selected = new();
        allFriendlyUnits = new();
        allOtherUnits = new();
        cam = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Begin the selection
        if (Input.GetMouseButtonDown(0))
        {
            selectBox.gameObject.SetActive(true);
            selectBox.sizeDelta = Vector3.zero;
            // Get bot start corner on near plane
            mouseStart = Input.mousePosition;
        }
        // Update selection
        else if (Input.GetMouseButton(0))
        {
            Vector2 area = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mouseStart;
            selectBox.anchoredPosition = mouseStart + area / 2f;
            selectBox.sizeDelta = area.Abs();
            Bounds bounds = new(selectBox.anchoredPosition, selectBox.sizeDelta);

            foreach (Unit u in allFriendlyUnits)
                AddUnitsInSelectionBox(allFriendlyUnits, ref newSelected, bounds);

            // No friendly units were selected, so instead check if there's an enemy unit to select
            if (selected.Count == 0)
                AddUnitsInSelectionBox(allOtherUnits, ref newSelected, bounds, true);
        }
        // End selection; retrieve all units in the selection box
        else if (Input.GetMouseButtonUp(0))
        {
            selectBox.gameObject.SetActive(false);
            if (newSelected.Count > 0)
            {
                foreach (Unit u in selected)
                    u.Deselect();

                selected = new(newSelected);
                newSelected.Clear();

                foreach (Unit u in selected)
                    u.Select();
            }
            
        }
    }

    /// <summary>
    /// Adds all units from the given set that lie within the given bounds to the selected units list
    /// </summary>
    /// <param name="unitsToCheck">The set of units to check for selection</param>
    /// <param name="selectedList">The list to append units inside the selection box to</param>
    /// <param name="bounds">The selection box bounds</param>
    /// <param name="oneUnitOnly">If true, only populates the selected list with the first unit that lies in the selection box.</param>
    void AddUnitsInSelectionBox(HashSet<Unit> unitsToCheck, ref List<Unit> selectedList, Bounds bounds, bool oneUnitOnly = false)
    {
        // Simple AABB test to see if the unit's inside the selection box
        foreach (Unit u in unitsToCheck)
        {
            Vector2 unitPos = cam.WorldToScreenPoint(u.transform.position);
            if (unitPos.x > bounds.min.x && unitPos.x < bounds.max.x && unitPos.y > bounds.min.y && unitPos.y < bounds.max.y)
            {
                selectedList.Add(u);
                u.Select();
                if (oneUnitOnly) break;
            }
        }
    }
}
