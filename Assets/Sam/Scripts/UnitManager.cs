using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject[] playerUnits;

    private void Awake()
    {
        UpdatePlayerUnitsList();
    }

    public void UpdatePlayerUnitsList()
    {
        playerUnits = GameObject.FindGameObjectsWithTag("Player");
    }
}
