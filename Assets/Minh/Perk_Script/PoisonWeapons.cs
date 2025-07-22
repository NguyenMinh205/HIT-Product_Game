using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWeapons : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("poison_damage", 1, -1));
    }
}