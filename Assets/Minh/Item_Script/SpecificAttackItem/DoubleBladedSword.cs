using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBladedSword : AttackWithBuff
{
    private int damage = 20;
    public int Damage { get { return damage; } set { damage = value; } }
    private int buffVal = -5;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Buff(Player player)
    {
        player._CharacterStatModifier.ChangeCurHP(buffVal);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            AttackEnemy(enemy);
        }

        if (player.TryGetComponent<Player>(out var playerComponent))
        {
            Buff(playerComponent);
        }
    }
}
