using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enraged : IPerkAction
{
    public void Execute()
    {
        GamePlayController.Instance.PlayerController.startRoundBuffs.Add(new StartRoundBuffInfo("enraged_effect", 1, -1));
    }
}