using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : AttackItem
{
    private int damage = 10;
    public int Damage { get { return damage; } set { damage = value; } }
    private int curDamage = 0;

    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)curDamage);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            curDamage = CalculateDamageWithCrit(player, damage);
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

    public override void Upgrade()
    {
        damage *= 2;
    }
}
