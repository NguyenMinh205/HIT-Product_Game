using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaItem", menuName = "Gacha/GachaItem", order = 1)]
public class GachaItem : ScriptableObject
{
    public Sprite icon;
    public string itemName;
}
