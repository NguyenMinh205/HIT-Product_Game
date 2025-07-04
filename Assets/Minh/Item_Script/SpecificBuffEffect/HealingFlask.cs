using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFlask : BuffItem
{
    private int buffVal = 10;
    public int BuffVal {  get { return buffVal; } set { buffVal = value; } }
    public override void Buff(Player player)
    {
        player._CharacterStatModifier.ChangeCurHP(buffVal);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        } 
            
    }
}
