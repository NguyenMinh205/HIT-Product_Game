using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepCuts : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCriticalDamage(0.2f);
    }
}