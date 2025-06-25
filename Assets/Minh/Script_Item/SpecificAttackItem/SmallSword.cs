using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSword: AttackItem
{
    public override void AttackEnemy(Enemy enemy, float damage = 0)
    {
        if (enemy == null) return;
        //Thêm logic nếu chỉ có mình đồ này được gắp lên thì x2 damage
        enemy.ReceiverDamage((int)damage);
    }
}
