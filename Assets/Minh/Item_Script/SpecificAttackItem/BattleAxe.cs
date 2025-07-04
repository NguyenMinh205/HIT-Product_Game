using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAxe : AttackWithEffect
{
    private int damage = 15;
    public int Damage { get { return damage; } set { damage = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Effect(Enemy enemy)
    {
        //Xóa toàn bộ khiên của enemy
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            Effect(enemy);
            AttackEnemy(enemy);
        }
    }
}
