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

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            AttackEnemy(enemy);
        }    
    }
    
    public void Execute(GameObject player, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Execute(player, target);
            }
        } 
            
    }
}
