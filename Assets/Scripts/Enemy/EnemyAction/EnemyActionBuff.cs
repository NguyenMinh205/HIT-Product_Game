using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EnemyActionBuff : MonoBehaviour
{

}

public class IncreaseDamage : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.Damage += 5;
    }
}
public class GetGasPoison : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("poison_effect", 10, 3);
    }
}

public class DoubleDamage : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.actions.RemoveAt(0);
        enemy.AddBuffEffect("double_damage_each_turn", 2, -1);
    }
}

public class Explosive : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}
public class DodgeAttackByPlayer : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}
public class ThornsCounterDamage : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
       
    }
}
public class SuckBlood : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
       
    }
}
public class UpCoinWhenKill : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}
