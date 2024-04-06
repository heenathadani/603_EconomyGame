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
    public BiomassBank biomassBank;

    public delegate void IntroductionHandler();
    public event IntroductionHandler NextIntroduction;


    // Start is called before the first frame update
    void Start()
    {
        capital = GameObject.FindWithTag("CapitalUnit").GetComponent<Unit>();
        tutorialText.text = "Hey, you! Yeah, I created you. The Glorbomacians are up to no good again, and I needed a genius solution. So, behold! You're a sentient AI designed by yours truly to unleash havoc from within their ranks. Yeah, I know, it's a typical Tuesday for me. (Press Enter to Continue)";
        NextIntroduction += Introduction1;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter is being pressed");
            NextIntroduction?.Invoke();
        }
    }

    void Introduction1()
    {
        NextIntroduction -= Introduction1;
        tutorialText.text = "Anyway, forget the technical jargon. Your job is simple: control your virus cells and wipe 'em out from the inside. (Press Enter to Continue)";
        NextIntroduction += Introduction2;
    }

    void Introduction2()
    {
        NextIntroduction -= Introduction2;
        tutorialText.text = "See that blobby thing in the center of your view? That's your queen cell. It's like the mother ship, but cooler. It can spawn other cells, but it needs biomass. Lucky for us, we've got some to spare. Click on the queen cell.";
        capital.OnSelected += ShowCapitalTutorial;
    }

    void ShowCapitalTutorial(Unit u)
    {
        capital.OnSelected -= ShowCapitalTutorial;
        tutorialText.text = "Great, now spawn yourself a worker unit";
        capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted += ShowWorkerTutorial;
    }
    void ShowWorkerTutorial()
    {
        capital.GetAbility("Spawn Worker Spore").OnAbilityExecuted -= ShowWorkerTutorial;
        tutorialText.text = "Nice! Now, click on that little guy and send him over to that weird-looking thing on your left. It's a biomass buffet. Right-click to move to it.";
        biomassUnit.OnBeginExtract += ShowBiomassTutorial;
    }
    void ShowBiomassTutorial()
    {
        //Invoke("ShowText", 8);
        biomassUnit.OnBeginExtract -= ShowBiomassTutorial;

        tutorialText.text = "Great, you should be collecting some, but we need more biomass. There's some scattered around the map. Go fetch it!";

        biomass2.gameObject.SetActive(true);
        biomass3.gameObject.SetActive(true);
        //Invoke("ShowEndTutorial", 8);
        //Invoke("ShowEndText", 5);
        //if(biomassBank.Biomass > 1000)
        //    sceneManager.LoadScene(6);
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



