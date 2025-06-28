using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : AttackWithBuffItem
{
    public override void AttackEnemy(Enemy enemy, float damage = 0)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Buff(Player player, float val = 0)
    {
        //Tăng tỉ lệ chí mạng lên 50% khi tấn công bằng đồ này
    }
 
    public override void Execute(GameObject player = null, GameObject target = null, float value = 0)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        Player curPlayer = player?.GetComponent<Player>();
        if (curPlayer != null) Buff(curPlayer);
        if (enemy != null) AttackEnemy(enemy, value);
    }
}
