using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    Unit capital;
    public Biomass biomassUnit;
    public Biomass biomass2;
    public Biomass biomass3;
    SceneManagers sceneManager;
    BiomassBank biomassBank;

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
        Invoke("ShowText", 8);
        biomassUnit.OnBeginExtract -= ShowBiomassTutorial;
        biomass2.gameObject.SetActive(true);
        biomass3.gameObject.SetActive(true);
        Invoke("ShowEndTutorial", 8);
        Invoke("ShowEndText", 5);
        if(biomassBank.Biomass > 1000)
            sceneManager.LoadScene(6);
    }
    void ShowText()
    {
        tutorialText.text = "The worker is now merged with the Biomass and has begun extracting from it.\nThe Top Left corner of the screen indicates your Biomass count. \nIt is your primary resource for everything.";
    }
    void ShowEndTutorial()
    {
        tutorialText.text = "Amazing! Look for 2 more biomass pockets, spawn a worker and morph them to a melee spore structure!";
    }
    void ShowEndText()
    {
        tutorialText.text = "Congratulations! \nYou are now ready to spread your virus across the world!";
        
    }

}
