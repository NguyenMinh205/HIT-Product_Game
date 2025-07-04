using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedMace : AttackItem
{
    private int damage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        enemy.ReceiverDamage((int)damage);
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (player == null || target == null || player.TryGetComponent<Player>(out var playerComponent) || target.TryGetComponent<Enemy>(out var enemy)) return;
        damage = (int)playerComponent.Stats.shield;
        playerComponent._CharacterStatModifier.ChangeShield(-(damage * 0.2f));
        AttackEnemy(enemy);
    }
}
