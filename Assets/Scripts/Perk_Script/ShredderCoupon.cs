using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderCoupon : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeShredderFreeTurn(1);
    }
}