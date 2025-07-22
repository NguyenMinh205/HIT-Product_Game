using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspiciousGreenRod : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("add_poison_to_enemy", 3, 3));
    }
}