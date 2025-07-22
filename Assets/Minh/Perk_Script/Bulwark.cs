using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulwark : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_shield_start_round", 15, -1));
    }
}
