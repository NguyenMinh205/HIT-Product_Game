using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : AttackWithBuffItem
{
    public override void AttackEnemy(Enemy enemy, float damage = 0)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Buff(Player player, float val = 0)
    {
        //Random giá trị hồi máu cho player trong khoảng từ 2 - 5 giá trị
    }

    public override void Execute(GameObject player = null, GameObject target = null, float value = 0)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        Player curPlayer = player?.GetComponent<Player>();
        if (enemy != null) AttackEnemy(enemy, value);
        if (curPlayer != null) Buff(curPlayer);
    }

    public void Execute(GameObject player = null, GameObject target = null, float attackVal = 0, float buffVal = 0)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        Player curPlayer = player?.GetComponent<Player>();
        if (enemy != null) AttackEnemy(enemy, attackVal);
        if (curPlayer != null) Buff(curPlayer, buffVal);
    }
}
