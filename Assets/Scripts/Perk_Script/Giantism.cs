using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giantism : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeMaxHP(20);
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCurHP(20);
    }
}