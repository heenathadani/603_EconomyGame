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
            // Set select box dimensions
            Vector2 area = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mouseStart;
            selectBox.anchoredPosition = mouseStart + area / 2f;
            selectBox.sizeDelta = area.Abs();

            // Get units in selection area
            Bounds bounds = new(selectBox.anchoredPosition, selectBox.sizeDelta);
            foreach (Unit u in GetUnitsInSelectionBox(allFriendlyUnits, bounds))
            {
                u.SetShowSelection(true);
            }
        }
        // End selection; retrieve all units in the selection box
        else if (Input.GetMouseButtonUp(0))
        {
            selectBox.gameObject.SetActive(false);

            // Get units in selection area. If no friendlies, check if there's another type of unit to select.
            Bounds bounds = new(selectBox.anchoredPosition, selectBox.sizeDelta);
            newSelected = GetUnitsInSelectionBox(allFriendlyUnits, bounds);
            if (selected.Count == 0)
                newSelected = GetUnitsInSelectionBox(allOtherUnits, bounds, true);

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
    List<Unit> GetUnitsInSelectionBox(HashSet<Unit> unitsToCheck, Bounds bounds, bool oneUnitOnly = false)
    {
        List<Unit> selectedUnits = new();

        // Simple AABB test to see if the unit's inside the selection box
        foreach (Unit u in unitsToCheck)
        {
            Vector2 unitPos = cam.WorldToScreenPoint(u.transform.position);
            if (unitPos.x > bounds.min.x && unitPos.x < bounds.max.x && unitPos.y > bounds.min.y && unitPos.y < bounds.max.y)
            {
                selectedUnits.Add(u);
                u.Select();
                if (oneUnitOnly) break;
            }
        }
        return selectedUnits;
    }
}
