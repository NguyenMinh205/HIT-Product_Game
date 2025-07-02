using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShield : DefendItem
{
    public override void Defend(Player player, float shield = 0)
    {
        int index = Random.Range(0, 3);
        if (index == 0)
            player._CharacterStatModifier.ChangeShield(shield);
        else
            player._CharacterStatModifier.ChangeShield(shield * 2);
    }

    public override void Execute(GameObject player, GameObject target, float value = 0)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Defend(playerComponent, value);
        }
    }
}
