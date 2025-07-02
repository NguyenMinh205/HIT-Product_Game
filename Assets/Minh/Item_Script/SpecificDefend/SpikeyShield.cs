using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyShield : DefendWithBuff
{
    public override void Buff(Player player, float value = 0)
    {
        player.AddBuffEffect("thorns_damage", 5, -1);
    }

    public override void Defend(Player player, float shield = 0)
    {
        player._CharacterStatModifier.ChangeShield(shield);
    }

    public override void Execute(GameObject player, GameObject target, float value = 0)
    {
        if (player != null && player.TryGetComponent<Player>(out var playerComponent))
        {
            Defend(playerComponent, value);
            Buff(playerComponent, value);
        }
    }

    public override void Execute(GameObject player, GameObject target, float defendValue = 0, float buffValue = 0)
    {
        throw new System.NotImplementedException();
    }
}
