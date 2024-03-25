using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject abilitiesBoard;
    public BiomassBank biomassBank;

    List<Button> abilityButtons;

    // Start is called before the first frame update
    void Start()
    {
        abilityButtons = new(abilitiesBoard.GetComponentsInChildren<Button>(true));
        SelectionManager.OnUnitsSelected += OnUnitsSelected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUnitsSelected(List<Unit> units)
    {
        UnitAbility[] abilities = units[0].GetComponents<UnitAbility>();
        int i = 0;
        for (; i < abilities.Length; i++)
        {
            if (i < abilityButtons.Count)
            {
                UnitAbility a = abilities[i];
                
                abilityButtons[i].onClick.RemoveAllListeners();
                abilityButtons[i].onClick.AddListener(() =>
                    {
                        if (a.timer <= 0f && biomassBank.SpendBiomass(a.cost))
                        {
                            a.timer = a.cooldown;
                            a.Execute();
                        }
                    });
                abilityButtons[i].GetComponent<Image>().sprite = a.abilitySprite;
                abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = a.abilityName;
                abilityButtons[i].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"A selected unit has more than {abilityButtons.Count} abilities! Only the first {abilityButtons.Count} abilities on this unit will be usable.");
                break;
            }
        }
        for (; i < abilityButtons.Count; i++)
            abilityButtons[i].gameObject.SetActive(false);
    }
}
