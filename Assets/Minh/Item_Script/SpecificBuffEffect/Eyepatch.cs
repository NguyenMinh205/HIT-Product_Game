using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyepatch : BuffItem
{
    private int buffVal = 50;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }

    public override void Buff(Player player)
    {
        player._CharacterStatModifier.ChangeCriticalChance(buffVal);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }

    }
}
