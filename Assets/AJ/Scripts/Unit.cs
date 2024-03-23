using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum Hostility
{
    Friendly,
    Neutral,
    Hostile
}

[RequireComponent(typeof(Collider), typeof(NavMeshAgent))]
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

    [Tooltip("Hostility of this unit.")]
    public Hostility hostility = Hostility.Friendly;

    public GameObject selectionPrefab;
    GameObject selectIcon;
    NavMeshAgent agent;

    float hp;
    bool selected = false;
    float stopCD = 0.2f;
    float stopTmr = 0;

    public BiomassBank biomassBank;
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
        agent = GetComponent<NavMeshAgent>();

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
        stopTmr += Time.deltaTime;

        if (stopTmr >= stopCD && agent.velocity.sqrMagnitude <= Mathf.Pow(agent.speed * 0.1f, 2))
        {
            agent.isStopped = true;
        }
    }

    public void Select()
    {
        if (!selected)
        {
            selected = true;
            SetShowSelection(true);
            OnSelected?.Invoke(this);
        }
    }
    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            SetShowSelection(false);
            OnDeselected?.Invoke(this);
        }
    }
    public void SetShowSelection(bool showSelection)
    {
        if (selectIcon)
            selectIcon.SetActive(showSelection);
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

    public void MoveTo(Vector3 destination)
    {
        stopTmr = 0f;
        agent.isStopped = false;
        agent.destination = destination;
    }
    public void Follow(Unit other)
    {
        stopTmr = 0f;
    }

    public void OnTriggerEnter(Collider other)
    {
        Biomass biomass = other.GetComponent<Biomass>();
        if (biomass)
        {
            biomass.CollectResource(this);
        }
    }
    public void CollectBiomass(int amount) 
    {
        biomassBank.AddBiomass(amount); 
    }
}
