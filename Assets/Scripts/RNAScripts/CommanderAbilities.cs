using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum CommanderAbilityType
{
    CellRegeneration,
    WeakeningSerum
}

public class CommanderAbility : MonoBehaviour
{
    public float timer = 0;

    void Update()
    {
        timer -= Time.deltaTime;
    }
}

public struct CommanderAbilityData
{
    public string name;
    public Action function;
    public int cost;
    public float cooldown;
    public bool selected;
}
