using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Hostility
{
    Friendly,
    Neutral,
    Hostile
}

public class Unit : MonoBehaviour
{
    // Event handlers
    public delegate void KilledHandler(Unit destroyedUnit);
    public event KilledHandler OnKilled;

    public delegate void SelectedHandler(Unit selectedUnit);
    public event SelectedHandler OnSelected;
    public delegate void DeselectedHandler(Unit deselectedUnit);
    public event DeselectedHandler OnDeselected;

    [Tooltip("The maximum HP of this unit. Set to 0 if indestructible.")]
    public float maxHP = 0f;
    [Tooltip("If this unit is immune to damage. If Max HP was set to 0, this does not matter.")]
    public bool immune = false;
    public float moveSpeed = 5f;

    [Tooltip("Hostility of this unit.")]
    public Hostility hostility = Hostility.Friendly;

    public GameObject selectionPrefab;
    GameObject selectIcon;

    float hp;
    bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        // Add this unit to the list ofselectable units
        switch (hostility)
        {
            case Hostility.Friendly:
                SelectionManager.allFriendlyUnits.Add(this);
                break;
            default:
                SelectionManager.allOtherUnits.Add(this);
                break;
        }

        hp = maxHP;
        if (hp == 0f) immune = true;

        if (selectionPrefab)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                selectIcon = Instantiate(selectionPrefab, hit.point + new Vector3(0, 0.01f, 0), Quaternion.Euler(90, 0, 0));
                selectIcon.SetActive(false);
                selectIcon.name = $"{gameObject.name} Selection Icon";
                selectIcon.transform.parent = transform;
                SetHostility(hostility);
            }
        }
        else
        {
            Debug.LogWarning($"The Unit {gameObject.name} does not have a Selection Icon prefab set on its Unit component!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        if (!selected)
        {
            selected = true;
            if (selectIcon)
                selectIcon.SetActive(true);
            OnSelected?.Invoke(this);
        }
    }
    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            if (selectIcon)
                selectIcon.SetActive(false);
            OnDeselected?.Invoke(this);
        }
    }
    public void SetHostility(Hostility hostility)
    {
        this.hostility = hostility;

        if (selectIcon)
        {
            switch (hostility)
            {
                case Hostility.Friendly:
                    selectIcon.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case Hostility.Neutral:
                    selectIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case Hostility.Hostile:
                    selectIcon.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        hp = Mathf.Clamp(dmg, 0, maxHP);
        if (hp <= 0)
        {
            OnKilled?.Invoke(this);
            // Destroy after a delay
            Destroy(gameObject, 1f);
        }
    }
    public void Heal(float healAmt)
    {
        hp = Mathf.Clamp(healAmt, 0, maxHP);
    }
}
