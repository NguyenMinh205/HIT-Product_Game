using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalStrength : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_strength_start_round", 3, -1));
    }
}