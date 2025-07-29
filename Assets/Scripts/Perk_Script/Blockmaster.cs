using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockmaster : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeRetainedBlock(0.25f);
    }
}
