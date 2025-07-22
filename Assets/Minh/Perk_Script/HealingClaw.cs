using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingClaw : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_health_use_claw", 3, -1));
    }
}