using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RNAShop : MonoBehaviour
{
    public string jsonPath = "save_data/abilities.json";
    public RNABank rnaBank;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PurchaseAbility(int rnaAmount)
    {
            rnaBank.RNA += rnaAmount;
    }
}
