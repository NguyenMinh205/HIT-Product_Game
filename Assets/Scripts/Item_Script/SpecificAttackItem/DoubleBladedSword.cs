using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBladedSword : AttackWithBuff
{
    private int damage = 20;
    public int Damage { get { return damage; } set { damage = value; } }
    private int buffVal = -5;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    private int curDamage = 0;

    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(curDamage);
    }

    public override void Buff(Player player)
    {
        player.Stats.ChangeCurHP(buffVal);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            curDamage = CalculateDamageWithCrit(player, damage);
            AttackEnemy(enemy);
        }

        if (player != null)
        {
            Buff(player);
        }
    }

    public override void Upgrade()
    {
        damage += 10;
    }
}
