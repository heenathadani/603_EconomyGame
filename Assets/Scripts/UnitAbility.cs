using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class UnitAbility : MonoBehaviour
{
    public delegate void AbilityExecutedHandler();
    public event AbilityExecutedHandler OnAbilityExecuted;

    public int cost = 0;
    public float cooldown = 0f;
    public string abilityName = "Ability";
    public string description = "Ability Description";
    public UnitType requiredUnit = UnitType.None;
    public Sprite abilitySprite;

    float timer = 0f;

    const string defaultImgPath = "Assets/Art/Sprites/Ability_Default.png";

    private void Awake()
    {
        if (!abilitySprite)
        {
            Texture2D imgTex = new Texture2D(128, 128);
            if (imgTex.LoadImage(File.ReadAllBytes(defaultImgPath)))
            {
                abilitySprite = Sprite.Create(imgTex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0f, cooldown);
    }

    /// <summary>
    /// Called when the player clicks on the corresponding Unit Ability button.
    /// Override this method in your specific ability scripts. BE SURE TO CALL base.Execute() AT THE END
    /// The HUDController already handles checking the ability's cost and cooldown before executing,
    /// so no need to also do it here.
    /// </summary>
    public virtual bool Execute()
    {
        if (timer <= 0f)
        {
            timer = cooldown;
            OnAbilityExecuted?.Invoke();
            return true;
        }
        return false;
    }
}
