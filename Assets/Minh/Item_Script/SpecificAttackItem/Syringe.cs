using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : AttackItem
{
    private int damage = 5;
    public int Damage { get { return damage; } set { damage = value; } }
    private int damageIncrease = 3;
    public int DamageIncrease { get { return damageIncrease; } set { damageIncrease = value; } }
    private int maxDamage = 20;
    public int MaxDamage { get { return maxDamage; } set { maxDamage = value; } }
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(curDamage);
        if (damage < maxDamage)
        {
            damage += damageIncrease;
        }    
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy == null) return;
        curDamage = CalculateDamageWithCrit(player, damage);
        AttackEnemy(enemy);
    }
}
