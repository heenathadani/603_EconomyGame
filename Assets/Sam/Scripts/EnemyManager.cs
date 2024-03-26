using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyUnits;

    private void Awake()
    {
        UpdateEnemyUnitsList();
    }

    public void UpdateEnemyUnitsList()
    {
        enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
