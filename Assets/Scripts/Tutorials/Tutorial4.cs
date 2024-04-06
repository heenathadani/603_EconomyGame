using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial4 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    Unit capital;
    public Unit enemy;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Enemies approach!!! \nSpawn a Worker Spore and morph it into a Melee Spore Structure. \nThis allows you to spawn Combat Spores from your Capital.";
        capital = GameObject.FindWithTag("CapitalUnit").GetComponent<Unit>();
        capital.GetAbility("Spawn Melee Spore").OnAbilityExecuted += () => tutorialText.text = "Great! \nSelect your new Combat Spore and Right Click the enemy to defeat it.";
        enemy.OnKilled += (Unit u) => tutorialText.text = "Very good! \nNow, collect the rest of the Biomass.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
