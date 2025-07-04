using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Difficulty
{
    public Sprite describeDifficulty;
    [TextArea(1, 3)]
    public string level;
    [TextArea(1, 3)]
    public string levelDescription;
    public float extraDamagePercent;
    public float extraHealthPercent;
}
