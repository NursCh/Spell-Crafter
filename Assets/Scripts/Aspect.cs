using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Aspect 
{
    public string name;             // Name of the aspect
    public Sprite icon;            // Icon for the aspect
    public int tier;               // Tier of the aspect// Reference to the aspect's prefab

    public Aspect(GameObject prefab, int tier)
    {
        this.tier = tier;
        this.name = prefab.name;
        var spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            this.icon = spriteRenderer.sprite;
        }
    }
}
