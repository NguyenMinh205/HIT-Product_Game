using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackWithBuff : IItemAction
{
    public abstract void AttackEnemy(Enemy enemy);

    public abstract void Buff(Player player);

    public abstract void Execute(Player player, Enemy target);
}
