using System;
using System.Collections.Generic;

public enum UnitType
{
    // Default
    None,

    // Structure unit types
    Capital,
    MeleeUnitStructure,
    DecoyBeacon,

    // "Spore" unit types
    WorkerUnit,
    MeleeUnit
}

public static class GameUtilities
{
    /// <summary>
    /// Capitalizes the first letter, and adds spaces before each subsequent uppercase letter
    /// </summary>
    /// <param name="type">The Unit Type to turn into a displayed string</param>
    /// <returns>The type as a displayable string</returns>
    public static string GetDisplayed(UnitType type)
    {
        List<char> text = new(type.ToString().ToCharArray());
        text[0] = char.ToUpper(text[0]);
        for (int i = text.Count - 1; i > 0; i--)
        {
            if (char.IsUpper(text[i]))
                text.Insert(i, ' ');
        }

        return new string(text.ToArray());
    }
}