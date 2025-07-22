using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_spikes_start_round", 5, -1));
    }
}