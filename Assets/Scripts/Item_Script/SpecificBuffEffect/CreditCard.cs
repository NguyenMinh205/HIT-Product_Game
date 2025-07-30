using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditCard : AttackWithBuff
{
    private int damage = 1;
    public int Damage { get { return damage; } set { damage = value; } }
    private int damageIncrease = 2;
    public int DamageIncrease { get { return damageIncrease; } set { damageIncrease = value; } }
    private int buffVal = 5;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }

    public override void AttackEnemy(Enemy enemy)
    {
        enemy.ReceiverDamage(damage);
        damage += damageIncrease;
    }

    public override void Buff(Player player)
    {
        player.Stats.ChangeCoin(buffVal);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
        if (enemy != null)
        {
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        damage *= 2;
        damageIncrease *= 2;
    }
}
