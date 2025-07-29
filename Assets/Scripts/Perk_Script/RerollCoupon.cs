using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollCoupon : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeRerollFreeTurn(1);
    }
}