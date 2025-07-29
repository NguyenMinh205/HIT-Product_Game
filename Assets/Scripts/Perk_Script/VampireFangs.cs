using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireFangs : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeDamageAbsorb(0.05f);
    }
}