using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string id;
    public string name;
    [TextArea(1,5)]
    public string description;
    [TextArea(1, 5)]
    public string abilityDescription;
    public List<string> startingItems;
    public List<Sprite> skins;
    public List<ItemInventory> initialItems;
}