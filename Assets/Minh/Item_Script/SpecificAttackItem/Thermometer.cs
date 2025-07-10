using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : AttackItem
{
    private int multiDamage = 1;
    public int MultiDamage { get; set; }
    private int damage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(damage);
    }

    public override void Execute(Player player, Enemy target)
    {
        if (player != null && target != null)
        {
            float lostHP = player.Stats.MaxHP - player.Stats.CurrentHP;
            damage = multiDamage * ((int)lostHP / 2);
            damage = CalculateDamageWithCrit(player, damage);
            AttackEnemy(target);
        } 
    }
}
