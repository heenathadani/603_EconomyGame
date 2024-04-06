using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial3 : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Use the Arrow Keys or move the mouse to the edges of the screen \nto move the camera. \nKeep collecting that Biomass!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
