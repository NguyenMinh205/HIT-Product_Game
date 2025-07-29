using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingAccount : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("buff_coin_end_round", 50, -1));
    }
}