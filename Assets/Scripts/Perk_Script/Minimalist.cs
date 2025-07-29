using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimalist : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_strength_by_items", 10, -1));
    }
}