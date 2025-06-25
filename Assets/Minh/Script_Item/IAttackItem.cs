using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackItem
{
    public abstract void AttackEnemy(Enemy enemy, float damage = 0);
}
