using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyArmor : BuffItem
{
    private int buffVal = 5;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }

    public override void Buff(Player player)
    {
        player.AddBuffEffect("double_shield", buffVal, -1);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }
    }
}
