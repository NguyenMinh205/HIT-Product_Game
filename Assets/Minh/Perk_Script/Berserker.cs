using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("add_strength_take_damage", 1, -1));
    }
}
