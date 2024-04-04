using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public struct AbilityData
{
    public List<string> abilities;
    public List<int> selectedIndices;
}

public class AbilitySelector : MonoBehaviour
{
    public int maxSelections = 3;

    Button[] abilityButtons;
    UnitAbility[] abilities;
    AbilityData data;

    public string jsonPath = "save_data/abilities.json";
    public RNABank rnaBank;
    public TextMeshProUGUI errorText;

    // Start is called before the first frame update
    void Start()
    {
        abilityButtons = GetComponentsInChildren<Button>();
        abilities = GetComponentsInChildren<UnitAbility>();

        // read in all unlocked abilities
        if (File.Exists(jsonPath))
        {
            data = JsonUtility.FromJson<AbilityData>(File.ReadAllText(jsonPath));
        }
        else
        {
            data = new()
            {
                abilities = new List<string>(),
                selectedIndices = new List<int>(),
            };
        }

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            UnitAbility a = abilities[i];

            TextMeshProUGUI[] texts = abilities[i].GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = a.abilityName;
            texts[1].text = a.description;

            // Set the button behavior and text
            if (data.selectedIndices.Contains(i)) // purchased and selected
            {
                texts[2].text = "Deselect";

                int j = i;
                abilityButtons[i].onClick.AddListener(() =>
                {
                    DeselectAbility(j);
                });
            }
            else if (data.abilities.Contains(a.GetType().ToString())) // purchased, but not selected
            {
                texts[2].text = "Select";

                int j = i;
                abilityButtons[i].onClick.AddListener(() =>
                {
                    SelectAbility(j);
                });
            }
            else // not purchased
            {
                texts[2].text = $"Purchase ({a.cost} RNA)";

                int j = i;
                abilityButtons[i].onClick.AddListener(() =>
                {
                    PurchaseAbility(j);
                });
            }
        }
    }

    void PurchaseAbility(int index)
    {
        UnitAbility a = abilities[index];
        if (rnaBank.RNA >= a.cost)
        {
            rnaBank.RNA -= a.cost;
            data.abilities.Add(a.GetType().ToString());
            abilityButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = "Select";
            abilityButtons[index].onClick.RemoveAllListeners();
            abilityButtons[index].onClick.AddListener(() => SelectAbility(index));
            RNABank.SaveTo(jsonPath, data);
        }
        else
        {
            AnimateError("Not enough RNA to purchase this Commander Ability.");
        }
    }
    void SelectAbility(int index)
    {
        if (data.selectedIndices.Count < maxSelections)
        {
            data.selectedIndices.Add(index);
            abilityButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = "Deselect";
            abilityButtons[index].onClick.RemoveAllListeners();
            abilityButtons[index].onClick.AddListener(() => DeselectAbility(index));
            RNABank.SaveTo(jsonPath, data);
        }
        else
        {
            AnimateError($"Cannot have more than {maxSelections} Commander Abilities selected.");
        }
    }
    void DeselectAbility(int index)
    {
        if (data.selectedIndices.Remove(index))
        {
            abilityButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = "Select";
            abilityButtons[index].onClick.RemoveAllListeners();
            abilityButtons[index].onClick.AddListener(() => SelectAbility(index));
            RNABank.SaveTo(jsonPath, data);
        }
        else
        {
            AnimateError("An unexpected error has occurred.");
        }
    }

    void AnimateError(string msg)
    {
        StopCoroutine("AnimateErrorAlpha");

        errorText.text = msg;
        Color c = errorText.color;
        c.a = 1;
        errorText.color = c;
       
        StartCoroutine("AnimateErrorAlpha");
    }

    IEnumerator AnimateErrorAlpha()
    {
        Color c = errorText.color;
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * 0.5f;
            errorText.color = c;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        errorText.alpha = 0;
    }
}
