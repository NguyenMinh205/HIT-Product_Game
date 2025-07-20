using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bargain : IPerkAction
{
    public void Execute(Player player)
    {
        player.Stats.ChangePriceReduction(0.2f);
    }
}