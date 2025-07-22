using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenArmor : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_health_per_coin", 2, -1));
    }
}