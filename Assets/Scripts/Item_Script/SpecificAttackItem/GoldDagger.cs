using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDagger : AttackItem
{
    private int damage = 5;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    private int damagePerCoin = 1;
    private int coinsPerIncrement = 5;
    private int totalDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        enemy.ReceiverDamage(totalDamage);
    }

    public override void Execute(Player player, Enemy target)
    {
        if (player != null)
        {
            int playerCoins = player.Stats.Coin;
            int additionalDamage = (playerCoins / coinsPerIncrement) * damagePerCoin;
            totalDamage = damage + additionalDamage;
        }

        if (target != null)
        {
            AttackEnemy(target);
        }    
    }

    public override void Upgrade()
    {
        damage = 10;
        damagePerCoin = 2;
    }
}
