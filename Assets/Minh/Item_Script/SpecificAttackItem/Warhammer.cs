using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhammer : AttackWithBuff
{
    private int damage = 15;
    public int Damage { get { return damage; } set { damage = value; } }
    private int buffVal = 10;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(curDamage);
    }

    public override void Buff(Player player)
    {
        player.Stats.ChangeShield(buffVal);
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
}
