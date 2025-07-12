using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackItem : IItemAction
{
    public abstract void AttackEnemy(Enemy enemy);

    public abstract void Execute(Player player, Enemy target);

    public abstract void Upgrade();

    protected int CalculateDamageWithCrit(Player player, int baseDamage)
    {
        if (player == null || player.Stats == null) return baseDamage;

        float criticalChance = player.Stats.CriticalChance;
        float criticalDamage = player.Stats.CriticalDamage;

        if (Random.value <= criticalChance)
        {
            float critDamage = baseDamage * criticalDamage;
            Debug.Log($"Chí mạng! Sát thương tăng từ {baseDamage} lên {critDamage}");
            return (int)critDamage;
        }

        return baseDamage;
    }
}
