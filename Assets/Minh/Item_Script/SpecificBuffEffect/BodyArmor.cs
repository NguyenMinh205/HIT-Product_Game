using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyArmor : BuffItem
{
    public override void Buff(Player player, float value = 0)
    {
        player.AddBuffEffect("double_shield", 5, -1);
    }

    public override void Execute(GameObject player, GameObject target, float value = 0)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent, value);
        }
    }
}
