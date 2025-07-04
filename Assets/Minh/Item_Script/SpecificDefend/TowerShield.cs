using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShield : DefendItem
{
    private int shield = 25;
    public int Shield { get { return shield; } set { shield = value; } }
    public override void Defend(Player player)
    {
        player._CharacterStatModifier.ChangeShield(shield);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Defend(playerComponent);
        }
    }
}
