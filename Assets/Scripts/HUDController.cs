using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject abilitiesBoard;
    public BiomassBank biomassBank;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI unitNameText;
    Unit focusedUnit;

    List<Button> abilityButtons;

    // Start is called before the first frame update
    void Start()
    {
        abilityButtons = new(abilitiesBoard.GetComponentsInChildren<Button>(true));
        SelectionManager.OnUnitSelectionChanged += OnUnitsSelected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUnitsSelected(List<Unit> units)
    {
        if (units.Count > 0)
        {
            UnitAbility[] abilities = units[0].GetComponents<UnitAbility>();
            int i = 0;

            // Only list abilities of friendly units
            if (units[0].Hostility == Hostility.Friendly)
            {
                for (; i < abilities.Length; i++)
                {
                    if (i < abilityButtons.Count)
                    {
                        UnitAbility a = abilities[i];

                        abilityButtons[i].onClick.RemoveAllListeners();
                        abilityButtons[i].onClick.AddListener(() =>
                        {
                            // Check if there is at least one of the required unit type for this ability
                            if (a.requiredUnit == UnitType.None || SelectionManager.allFriendlyUnits.ContainsKey(a.requiredUnit))
                            {
                                if (a.timer <= 0f && biomassBank.SpendBiomass(a.cost))
                                {
                                    a.timer = a.cooldown;
                                    a.Execute();
                                }
                                else
                                {
                                    errorText.text = $"Insufficient Biomass. Collect more Biomass.";
                                    GetComponent<Animator>().Play("ErrorTextFade", -1, 0);
                                }
                            }
                            else
                            {
                                errorText.text = $"{a.abilityName} requires a {GameUtilities.GetDisplayed(a.requiredUnit)}.";
                                GetComponent<Animator>().Play("ErrorTextFade", -1, 0);
                            }
                        });
                        abilityButtons[i].GetComponent<Image>().sprite = a.abilitySprite;
                        TextMeshProUGUI[] texts = abilityButtons[i].GetComponentsInChildren<TextMeshProUGUI>();
                        texts[0].text = a.abilityName;
                        texts[1].text = $"{a.cost} Biomass";
                        abilityButtons[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning($"A selected unit has more than {abilityButtons.Count} abilities! Only the first {abilityButtons.Count} abilities on this unit will be usable.");
                        break;
                    }
                }
            }
            
            for (; i < abilityButtons.Count; i++)
                abilityButtons[i].gameObject.SetActive(false);

            // Display the unit name
            unitNameText.text = $"{units[0].name} ({(int)units[0].currentHP}/{(int)units[0].maxHP} HP)";

            // Switch to listen to the new focused unit's Damage Taken event
            if (focusedUnit)
                focusedUnit.OnDamageTaken -= UpdateUnitNameDisplay;
            focusedUnit = units[0];
            focusedUnit.OnDamageTaken += UpdateUnitNameDisplay;
        }
        else
        {
            // disable all ability buttons
            unitNameText.text = "";
            foreach (Button b in abilityButtons)
            {
                b.onClick.RemoveAllListeners();
                b.gameObject.SetActive(false);
            }
        }
    }

    void UpdateUnitNameDisplay()
    {
        unitNameText.text = $"{focusedUnit.name} ({(int)focusedUnit.currentHP}/{(int)focusedUnit.maxHP} HP)";
    }
}
