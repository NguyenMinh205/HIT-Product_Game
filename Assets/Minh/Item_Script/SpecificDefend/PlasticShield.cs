using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticShield : DefendItem
{
    private int shield = 15;
    public int Shield { get { return shield; } set { shield = value; } }
    private int minShield = 3;
    public int MinShield { get { return minShield; } set { minShield = value; } }
    public override void Defend(Player player)
    {
        player._CharacterStatModifier.ChangeShield(shield);
        if (shield >= minShield)
        {
            shield -= minShield;
        }    
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Defend(playerComponent);
        }
    }
}
