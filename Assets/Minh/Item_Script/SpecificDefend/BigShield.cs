using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShield : DefendItem
{
    private int shield = 10;
    public int Shield { get { return shield; } set { shield = value; } }
    public override void Defend(Player player)
    {
        int index = Random.Range(0, 3);
        if (index == 0)
            player._CharacterStatModifier.ChangeShield(shield);
        else
            player._CharacterStatModifier.ChangeShield(shield * 2);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Defend(playerComponent);
        }
    }
}
