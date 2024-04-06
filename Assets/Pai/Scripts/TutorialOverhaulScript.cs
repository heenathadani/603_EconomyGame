using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static Unit;

public class TutorialOverhaulScript : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public Unit capital;
    public Biomass biomassUnit;

    public delegate void IntroductionHandler();
    public event IntroductionHandler NextIntroduction;


    // Start is called before the first frame update
    void Start()
    {
        tutorialText.enabled = true;
        tutorialText.text = "Hey, you! Yeah, I created you. The Glorbomacians are up to no good again, and I needed a genius solution. So, behold! You're a sentient AI designed by yours truly to unleash havoc from within their ranks. Yeah, I know, it's a typical Tuesday for me.";
        NextIntroduction += Introduction1;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            NextIntroduction?.Invoke();
        }
    }

    void Introduction1()
    {
        NextIntroduction -= Introduction1;
        tutorialText.text = "Anyway, forget the technical jargon. Your job is simple: control your virus cells and wipe 'em out from the inside.";
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
        tutorialText.text = "Nice! Now, click on that little guy and send him over to that weird-looking thing on your left. It's a biomass buffet. Right-click to collect it.";
        biomassUnit.OnBeginExtract += ShowBiomassTutorial;
    }

    void ShowBiomassTutorial()
    {
        biomassUnit.OnBeginExtract -= ShowBiomassTutorial;
        tutorialText.text = "Great, you should be collecting some, but we need more biomass. There's some scattered around the map. Go fetch it!";
    }
}
