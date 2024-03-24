using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class UnitAbility : MonoBehaviour
{
    public int cost = 0;
    public float cooldown = 0f;
    public string abilityName = "Ability";
    public string description = "Ability Description";
    public Texture2D image;

    const string defaultImgPath = "Assets/Art/Sprites/Ability_Default.png";

    // Start is called before the first frame update
    void Start()
    {
        if (!image)
        {
            image = new Texture2D(128, 128);
            image.LoadImage(File.ReadAllBytes(defaultImgPath));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Execute();
}
