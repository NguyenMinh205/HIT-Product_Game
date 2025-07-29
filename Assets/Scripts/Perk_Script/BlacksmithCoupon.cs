using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithCoupon : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeUpgradeFreeTurn(1);
    }
}
