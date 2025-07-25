using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyActionDeBuff;

public static class EnemyActionFactory 
{
    public static void GetActionEnemy(TypeEnemyAction type, Enemy enemy)
    {
        switch(type)
        {
            case TypeEnemyAction.Attack:
                Debug.Log("Enemy Attack Player");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Attack, enemy);
                EnemyActionAttack.Execute(enemy);
                break;

            case TypeEnemyAction.Defend:
                Debug.Log("Enemy Defend");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                EnemyActionDefend.Execute(enemy);
                break;

            case TypeEnemyAction.Heal:
                Debug.Log("Enemy Heal");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                EnemyActionHead.Execute(enemy);
                break;

            case TypeEnemyAction.Effect:
                Debug.Log("Enemy Effect");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                GetEffectEnemy(enemy.ID, enemy);
                break;

            case TypeEnemyAction.Buff:
                Debug.Log("Enemy Buff");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                GetBuffEnemy(enemy.ID, enemy);
                break;

            case TypeEnemyAction.DeBuff:
                Debug.Log("Enemy DeBuff");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                GetDeBuffEnemy(enemy.ID, enemy);
                break;

            case TypeEnemyAction.Drop:
                Debug.Log("Enemy Drop");
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                GetDropEnemy(enemy.ID, enemy);
                break;
        }
    }

    public static void GetBuffEnemy(string idEnemy, Enemy enemy)
    {
        IEnemyAction enemyAction = null;

        switch(idEnemy)
        {
            case "enemy08":
                Debug.Log("Enemy Get Gas Poison");
                enemyAction = new GetGasPoison();
                break;
            case "enemy09":
                enemyAction = new IncreaseDamage();
                break;

            case "enemy14":
                Debug.Log(" Enemy is Explosive");
                enemyAction = new Explosive();
                break;
            case "enemy15":
                enemyAction = new DodgeAttackByPlayer();
                break;
            case "enemy19":
                Debug.Log("Receiver Coin when kill enemy");
                enemyAction = new UpCoinWhenKill();
                break;
            case "enemy20":
                Debug.Log("Thief Action");
                enemyAction = new ThiefEffectMan();
                break;

            case "enemy22":
                Debug.Log("Enemy Set Posion With Action Attack");
                enemyAction = new SetPoisonWithAttack();
                break;

            case "boss02":
                Debug.Log("Boss Get Increase Damage");
                enemyAction = new DoubleDamage();
                break;

            case "boss06":
                Debug.Log("Boss Get Dodge Attack By Player Or Counter Attack by Player");
                enemyAction = new SetDodgeOrCounterAttack();
                break;
        }

        ExecuteAction(enemyAction, enemy);
    }

    public static void GetEffectEnemy(string idEnemy, Enemy enemy)
    {
        IEnemyAction enemyAction = null;

        switch (idEnemy)
        {
            case "enemy06":
                enemyAction = new CreateWaterInBoxEffect();
                break;
        }

        ExecuteAction(enemyAction, enemy);
    }
    public static void GetDeBuffEnemy(string idEnemy, Enemy enemy)
    {
        IEnemyAction enemyAction = null;
        switch (idEnemy)
        {
            case "enemy07":
                enemyAction = new GetPoison();
                break;

            case "enemy13":
                enemyAction = new GetPoison();
                break;
        }
            ExecuteAction(enemyAction, enemy);
    }
    public static void GetDropEnemy(string idEnemy, Enemy enemy)
    {
        IEnemyAction enemyAction = null;

        switch(idEnemy)
        {
            case "enemy04":
                Debug.Log("Enemy Drop Three Honey");
                enemyAction = new DropThreeHoney();
                break;

            case "enemy05":
                Debug.Log("Enemy Drop Three Fruit Poison");
                enemyAction = new DropThreeFruitPoison();
                break;

            case "enemy10":
                Debug.Log("Enemy Drop Three Fruit Thorn");
                enemyAction = new DropThreeFruitThorn();
                break;

            case "enemy11":
                Debug.Log("Enemy Drop Fluff In Box");
                enemyAction = new DropFluffInBox();
                break;
            
            case "enemy12":
                Debug.Log("Enemy Drop Three Fruit Poison");
                enemyAction = new DropThreeFruitPosionThorn();
                break;
        }
        ExecuteAction(enemyAction, enemy);
    }

    public static void ExecuteAction(IEnemyAction enemyAction, Enemy enemy)
    {
        if (enemyAction != null)
        {
            enemyAction.Execute(enemy);
        }
    }
}
