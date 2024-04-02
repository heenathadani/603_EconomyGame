using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    Unit capital;
    public Biomass biomassUnit;

    // Start is called before the first frame update
    void Start()
    {
        capital = GameObject.FindWithTag("CapitalUnit").GetComponent<Unit>();
        tutorialText.text = "Welcome! \nYou are a Virus. \nLeft Click your Capital present in the middle of the screen!";
        capital.OnSelected += ShowCapitalTutorial;
    }

    void ShowCapitalTutorial(Unit u)
    {
        capital.OnSelected -= ShowCapitalTutorial;
        tutorialText.text = "This is your Capital. If it dies, so do you. \nUse the Capital to spawn some Worker Spores.";
        capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted += ShowWorkerTutorial;
    }
    void ShowWorkerTutorial()
    {
        capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted -= ShowWorkerTutorial;
        tutorialText.text = "Good. \nLeft click to select your workers. Now, use your mouse to move the camera and look for Biomass. \nThen Right Click a Biomass Pocket to merge the worker with it.";
        biomassUnit.OnBeginExtract += ShowBiomassTutorial;
    }
    void ShowBiomassTutorial()
    {
        tutorialText.text = "The worker is now merged with the Biomass and has begun extracting from it.\nThe Top Left corner of the screen indicates your Biomass count. \nIt is your primary resource for everything.";
        biomassUnit.OnBeginExtract -= ShowBiomassTutorial;
        Invoke("ShowEndTutorial", 5);
        Invoke("ShowEndText", 2);
    }
    void ShowEndTutorial()
    {
        tutorialText.text = "Amazing! Look for 2 more biomass pockets, spawn a worker and morph them to a capital structure!";
    }
    void ShowEndText()
    {
        tutorialText.text = "Congratulations! \nYou are now ready to spread your virus across the world!";
        
    }

}
