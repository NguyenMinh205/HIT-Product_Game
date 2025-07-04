using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : AttackWithBuff
{
    private int damage = 15;
    public int Damage { get { return damage; } set { damage = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Buff(Player player)
    {
        //Tăng tỉ lệ chí mạng lên 50% khi tấn công bằng đồ này
    }
 
    public override void Execute(GameObject player = null, GameObject target = null)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        Player curPlayer = player?.GetComponent<Player>();
        if (curPlayer != null) Buff(curPlayer);
        if (enemy != null) AttackEnemy(enemy);
    }
}
