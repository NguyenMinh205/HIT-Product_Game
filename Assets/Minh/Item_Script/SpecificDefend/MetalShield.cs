using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalShield : DefendItem
{
    public override void Defend(Player player, float shield = 0)
    {
        player._CharacterStatModifier.ChangeShield(shield);
    }

    public override void Execute(GameObject player, GameObject target, float value = 0)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Defend(playerComponent, value);
        }
    }
}
