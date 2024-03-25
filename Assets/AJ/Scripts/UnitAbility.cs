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
    public Sprite sprite;

    const string defaultImgPath = "Assets/Art/Sprites/Ability_Default.png";

    // Start is called before the first frame update
    void Start()
    {
        if (!sprite)
        {
            Texture2D imgTex = new Texture2D(128, 128);
            if (imgTex.LoadImage(File.ReadAllBytes(defaultImgPath)))
            {
                sprite = Sprite.Create(imgTex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Execute();
}
