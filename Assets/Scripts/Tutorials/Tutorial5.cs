using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial5 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public float displayTime = 10f;

    public Image Rick;
    public Image Speech;


    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Amazing. You are ready. Infect this PUNY HUMAN.\nCollect 1000 Biomass to OBLITERATE THEIR IMMUNE SYSTEM. (Press Enter to Continue)";
    }

    // Update is called once per frame
    void Update()
    {
        /*        displayTime -= Time.deltaTime;
                if (displayTime <= 0f)
                {
                    tutorialText.text = "";
                    Destroy(gameObject);
                }*/

        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter is being pressed");
            tutorialText.enabled = false;
            Rick.enabled = false;
            Speech.enabled = false;
        }
    }
}
