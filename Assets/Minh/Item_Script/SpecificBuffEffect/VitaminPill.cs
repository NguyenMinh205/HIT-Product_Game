using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminPill : BuffItem
{
    public override void Buff(Player player)
    {
        int curDamageExtra = (int)player.Stats.damageIncrease;
        int shieldIncrease = curDamageExtra / 2;
        player._CharacterStatModifier.ChangeDamageExtra(shieldIncrease);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }    
    }
}
