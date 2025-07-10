using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : AttackWithEffect
{
    private int damage = 3;
    public int Damage { get { return damage; } set { damage = value; } }
    private int effectVal = 1;
    public int EffectVal { get { return effectVal; } set { effectVal = value; } }
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(curDamage);
    }

    public override void Effect(Enemy enemy)
    {
        //Add hiệu ứng độc cho enemy
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            curDamage = CalculateDamageWithCrit(player, damage);
            Effect(enemy);
            AttackEnemy(enemy);
        }
    }

    public void Execute(Player player, List<Enemy> targets)
    {
        foreach (Enemy target in targets)
        {
            if (target != null)
            {
                Execute(player, target);
            }
        }
    }
}
