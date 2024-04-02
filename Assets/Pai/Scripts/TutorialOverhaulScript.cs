using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialOverhaulScript : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    Unit capital;
    public Biomass biomassUnit;

    // Start is called before the first frame update
    void Start()
    {
        capital = GameObject.FindWithTag("CapitalUnit").GetComponent<Unit>();
        tutorialText.text = "Welcome! \nYou are a Virus. \nClick on your main cell to start.";
        capital.OnSelected += ShowCapitalTutorial;
    }

    void ShowCapitalTutorial(Unit u)
    {
        capital.OnSelected -= ShowCapitalTutorial;

        tutorialText.text = "Now spawn a worker spore at the panel below.";
        capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted += ShowWorkerTutorial;
    }
    void ShowWorkerTutorial()
    {
        capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted -= ShowWorkerTutorial;
        tutorialText.text = "Good. \nLeft click and Drag to select your workers. \nThen Right Click a Biomass Pocket to merge the worker with it.";
        biomassUnit.OnBeginExtract += ShowBiomassTutorial;
    }

    void ShowBiomassTutorial()
    {
        biomassUnit.OnBeginExtract -= ShowBiomassTutorial;
        tutorialText.text = "The worker is now merged with the Biomass and has begun extracting from it.\nThe Top Left corner of the screen indicates your Biomass count. \nIt is your primary resource for everything.";
    }
}
