using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public Unit capital;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Welcome! \nYou are a Virus. \nLeft Click your Capital.";
        capital.OnSelected += (Unit u) =>
        {
            tutorialText.text = "This is your Capital. If it dies, so do you. \nUse the Capital to spawn some Worker Spores.";
            capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted += () => tutorialText.text = "Good. \nLeft click and Drag to select your workers. \nThen Right Click a Biomass Pocket to collect the Biomass.";
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
