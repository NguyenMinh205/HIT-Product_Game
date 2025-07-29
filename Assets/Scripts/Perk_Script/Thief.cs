using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("add_coin_deal_damage", 1, -1));
    }
}