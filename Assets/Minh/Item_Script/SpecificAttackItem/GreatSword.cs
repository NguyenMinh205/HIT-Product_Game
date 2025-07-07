using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : AttackItem
{
    private int damage = 10;
    public int Damage { get { return damage; } set { damage = value; } }

    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Execute(Player player, Enemy target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
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
