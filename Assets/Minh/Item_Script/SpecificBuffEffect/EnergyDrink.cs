using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : BuffItem
{
    public override void Buff(Player player)
    {
        int curShield = (int)player.Stats.shield;
        int damageExtra = curShield / 4;
        player._CharacterStatModifier.ChangeDamageExtra(damageExtra);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }
    }
}
