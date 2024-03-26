using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial5 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public float displayTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Amazing. You are ready. \nInfect this PUNY HUMAN.\nCollect 1000 Biomass to OBLITERATE THEIR IMMUNE SYSTEM.";
    }

    // Update is called once per frame
    void Update()
    {
        displayTime -= Time.deltaTime;
        if (displayTime <= 0f)
        {
            tutorialText.text = "";
            Destroy(gameObject);
        }
    }
}
