using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public RectTransform selectBox;
    [Tooltip("The tendency of selected units to retain their formation when moving between destinations. " +
        "A lower values means a closer move destination to the group at which point they will break formation in order to get to said point.")]
    public float formationRetainment = 0.5f;

    List<Unit> newSelected, selected, pending;

    // Sets automatically handle dupes
    public static HashSet<Unit> allFriendlyUnits, allOtherUnits;

    Vector2 mouseStart;
    Camera cam;
    public BiomassBank biomassBank;
    private void Awake()
    {
        newSelected = new();
        selected = new();
        pending = new();
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
            List<Unit> newPending = GetUnitsInSelectionBox(allFriendlyUnits, bounds);
            foreach (Unit u in pending)
                u.SetShowSelection(false);
            pending = newPending;
            foreach (Unit u in pending)
                u.SetShowSelection(true);
        }
        // End selection; retrieve all units in the selection box
        else if (Input.GetMouseButtonUp(0))
        {
            pending.Clear();

            // Get units in selection area. If no friendlies, check if there's another type of unit to select.
            Bounds bounds = new(selectBox.anchoredPosition, selectBox.sizeDelta);
            newSelected = GetUnitsInSelectionBox(allFriendlyUnits, bounds);
            if (newSelected.Count == 0)
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

            selectBox.gameObject.SetActive(false);
        }

        // Handle moving units
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = cam.nearClipPlane;
            // Raycast against the environment or other units
            if (Physics.Raycast(transform.position, cam.ScreenToWorldPoint(mousePos) - transform.position, out RaycastHit hit, 500f, (1 << 0) | (1 << 3)))
            {
                if (hit.collider.TryGetComponent(out Unit target))
                {
                    foreach (Unit u in selected)
                        u.Follow(target);
                }
                else
                {
                    foreach (Unit u in selected)
                        u.MoveTo(hit.point);
                }
            }
        }
    }

    public void DepositBiomass(Unit worker)
    {
        biomassBank.AddBiomass(worker.biomass);
        worker.biomass = 0;
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

        // For a single click and no drag, raycast against Unit layer
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        if (Physics.Raycast(transform.position, cam.ScreenToWorldPoint(mousePos) - transform.position, out RaycastHit hit, 500f, 1 << 3))
        {
            selectedUnits.Add(hit.collider.GetComponent<Unit>());
            if (oneUnitOnly)
                return selectedUnits;
        }

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
