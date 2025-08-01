using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionBuff : MonoBehaviour
{

}

public class IncreaseDamage : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.DamageIncreased += 5;
    }
}
public class GetGasPoison : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("poison_gas", 3, 3);
    }
}

public class DoubleDamage : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("double_damage_each_turn", 2, -1);
    }
}

public class Explosive : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("explosive", 1, -1);
    }
}
public class DodgeAttackByPlayer : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("dodge", 1, 1);
    }
}
public class ThornsCounterDamage : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
       enemy.AddBuffEffect("thorns_counter_damage", 1, 1);
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
        enemy.actions.RemoveAt(0);
        enemy.AddBuffEffect("kill_receiver_coin", 30, -1);
    }
}
public class SetPoisonWithAttack : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("poison_damage", 3, 3);
    }
}

public class SetDodgeOrCounterAttack : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        int randomIndex = Random.Range(0, 2);
        if (randomIndex == 0)
        {
            Debug.Log("Enemy dodged the attack!");
            enemy.AddBuffEffect("dodge", 2, -1);
        }
        else
        {
            Debug.Log("Enemy counter-attacked!");
            enemy.AddBuffEffect("counter_attack", 2, -1);
        }
    }
}   

public class ThiefEffectMan : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.actions.RemoveAt(0);
        enemy.AddBuffEffect("thief_effect", 1, -1);
        enemy.AddBuffEffect("vanish", 1, -1);
    }
}

public class InscreaseDamageCoin : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        enemy.AddBuffEffect("increase_damage_by_coin", 1, -1);
    }
}
