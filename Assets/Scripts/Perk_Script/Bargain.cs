using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bargain : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangePriceReduction(0.2f);
    }
}