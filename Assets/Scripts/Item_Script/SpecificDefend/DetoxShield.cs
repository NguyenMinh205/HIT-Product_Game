using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetoxShield: DefendItem
{
    private float shield = 20f;

    public override void Execute(Player player, Enemy target)
    {
        if (player != null)
        {
            Defend(player);
        }
    }

    public override void Defend(Player player)
    {
        int additionalShield = 0;
        IBuffEffect poisonEffect = player.GetActiveEffect("poison_effect");
        if (poisonEffect != null)
        {
            additionalShield = (int)poisonEffect.Duration;
            player.RemoveBuffEffect(poisonEffect);
        }
        float totalShield = shield + additionalShield;
        player.Stats.ChangeShield(totalShield);
    }

    public override void Upgrade()
    {
        shield += 10;
    }
}
