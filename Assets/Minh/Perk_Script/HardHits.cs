using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardHits : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCriticalChance(0.1f);
    }
}