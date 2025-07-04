using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPoison : BuffItem
{
    public override void Buff(Player player)
    {
        player._CharacterStatModifier.DoubleDamageExtra();
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }
    }
}
