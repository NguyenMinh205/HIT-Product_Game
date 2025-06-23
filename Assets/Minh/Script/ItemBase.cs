using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemBase : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public Rarity itemRarity;
    [TextArea(1, 10)]
    public string description;
    public bool isStackable;
    public int maxStackSize = 1;

    public float damage;
    public float defense;
    public float healing;
}
