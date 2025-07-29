using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("add_spike_take_damage", 3, -1));
    }
}