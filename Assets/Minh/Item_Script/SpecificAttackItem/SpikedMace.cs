using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedMace : AttackItem
{
    private int damage = 0;
    private float shieldSpendPercent = 0.2f;
    public float ShieldSpendPercent { get; set; }
    public override void AttackEnemy(Enemy enemy)
    {
        enemy.ReceiverDamage(damage);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null && player != null)
        {
            damage = (int)player.Stats.Shield;
            player.Stats.ChangeShield(-damage * shieldSpendPercent);
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        shieldSpendPercent /= 2;
    }
}
