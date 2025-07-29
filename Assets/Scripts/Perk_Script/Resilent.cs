using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resilent : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_health_end_round", 10, -1));
    }
}