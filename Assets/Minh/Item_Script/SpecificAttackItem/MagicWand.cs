using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : AttackWithBuff
{
    private int damage = 5;
    public int Damage { get { return damage; } set { damage = value; } }
    private int minBuffVal = 2;
    public int MinBuffVal => minBuffVal;
    private int maxBuffVal = 5;
    public int MaxBuffVal => minBuffVal;

    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Buff(Player player)
    {
        player._CharacterStatModifier.ChangeCurHP(Random.Range(minBuffVal, maxBuffVal));
    }

    public override void Execute(GameObject player = null, GameObject target = null)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        Player curPlayer = player?.GetComponent<Player>();
        if (enemy != null) AttackEnemy(enemy);
        if (curPlayer != null) Buff(curPlayer);
    }
}
