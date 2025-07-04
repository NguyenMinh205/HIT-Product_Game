using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfStrength : BuffItem
{
    private int buffVal = 2;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    public override void Buff(Player player)
    {
        player._CharacterStatModifier.ChangeDamageExtra(buffVal);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }
    }
}
