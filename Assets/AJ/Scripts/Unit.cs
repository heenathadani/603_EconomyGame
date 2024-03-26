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

    public float attackDmg = 0;
    public float attackRange = 0;
    [Tooltip("How long the attack lasts. The unit cannot move until their attack completes.")]
    public float attackDuration = 0.25f;
    float atkDurTmr = 0f;
    public float attackRate = 0.15f;
    float atkTmr = 0f;

    public GameObject selectionPrefab;
    GameObject selectIcon;
    NavMeshAgent agent;

    [SerializeField] float hp;
    bool selected = false;
    float stopCD = 0.2f;
    float stopTmr = 0;
    bool attacking = false;
    [SerializeField] protected Unit followUnit;

    [SerializeField] protected UnitManager unitManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        unitManager = GameObject.FindWithTag("UnitManager").GetComponent<UnitManager>();
        unitManager.UpdatePlayerUnitsList();

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
                Vector3 extents = GetComponent<MeshRenderer>().localBounds.extents;
                float maxExtent = Mathf.Max(Mathf.Max(extents.x, extents.y), extents.z);
                selectIcon.transform.localScale = new(maxExtent, maxExtent, maxExtent);
                SetHostility(hostility);
            }
        }
        else
        {
            Debug.LogWarning($"The Unit {gameObject.name} does not have a Selection Icon prefab set on its Unit component!");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stopTmr += Time.deltaTime;
        atkTmr += Time.deltaTime;
        atkDurTmr += Time.deltaTime;

        if (stopTmr >= stopCD && agent.velocity.sqrMagnitude <= Mathf.Pow(agent.speed * 0.1f, 2))
        {
            agent.isStopped = true;
        }

        // If following a unit, keep updating the destination to move to
        if (followUnit)
        {
            // If the unit to follow should be attacked, stop moving and attack.
            if ((followUnit.transform.position - transform.position).sqrMagnitude <= attackRange * attackRange)
            {
                agent.isStopped = true;
                if (attacking && atkTmr >= attackRate)
                {
                    atkTmr = 0f;
                    atkDurTmr = 0f;
                    followUnit.TakeDamage(attackDmg);
                }
            }
            else if (atkDurTmr >= attackDuration)
            {
                agent.isStopped = false;
                agent.destination = followUnit.transform.position;
            }
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
        if (immune) return;

        hp = Mathf.Clamp(hp - dmg, 0, maxHP);
        if (hp <= 0)
        {
            OnKilled?.Invoke(this);
            // Destroy after a delay
            Destroy(gameObject, 1f); 
            unitManager.UpdatePlayerUnitsList();
        }
    }
    public void Heal(float healAmt)
    {
        hp = Mathf.Clamp(hp + healAmt, 0, maxHP);
    }

    public void MoveTo(Vector3 destination)
    {
        followUnit = null;
        stopTmr = 0f;
        agent.isStopped = false;
        agent.destination = destination;
    }

    /// <summary>
    /// Sets this unit to follow another unit indefinitely.
    /// If the unit's hostility does not match this one, it will attack it.
    /// </summary>
    /// <param name="other">The unit to follow or attack.</param>
    public void Follow(Unit other)
    {
        followUnit = other;
        attacking = other.hostility != hostility && !other.immune;
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
        GameObject.FindGameObjectWithTag("BiomassBank").GetComponent<BiomassBank>().AddBiomass(amount); 
    }

    protected float FindDistance(Vector2 targetLocation)
    {
        return Vector2.Distance(transform.position, targetLocation);
    }
}
