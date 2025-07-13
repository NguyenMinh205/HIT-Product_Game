using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                EnemyActionDefend.Execute(enemy);
                break;

            case TypeEnemyAction.Heal:
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                EnemyActionHead.Execute(enemy);
                break;

            case TypeEnemyAction.Effect:
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                GetEffectEnemy(enemy.ID, enemy);
                break;

            case TypeEnemyAction.Buff:
                ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Buff, enemy);
                GetBuffEnemy(enemy.ID, enemy);
                break;
        }
    }

    public static void GetBuffEnemy(string idEnemy, Enemy enemy)
    {
        IEnemyAction enemyAction = null;

        switch(idEnemy)
        {
            case "enemy01":
                break;
            case "enemy02":
                break;
            case "enemy03":
                break;
            case "enemy04":
                break;
            case "enemy05":
                break;
            case "enemy06":
                break;
            case "enemy07":
                break;
            case "enemy08":
                break;
            case "enemy09":
                enemyAction = new IncreaseDamage();
                break;
            case "enemy10":
                break;
            case "enemy11":
                break;
            case "enemy12":
                break;
            case "enemy13":
                break;
            case "enemy14":
                break;
            case "enemy15":
                break;
        }

        ExecuteAction(enemyAction, enemy);
    }

    public static void GetEffectEnemy(string idEnemy, Enemy enemy)
    {
        IEnemyAction enemyAction = null;

        switch (idEnemy)
        {
            case "enemy01":
                break;
            case "enemy02":
                break;
            case "enemy03":
                break;
            case "enemy04":
                enemyAction = new DropThreeHoney();
                break;
            case "enemy05":
                enemyAction = new DropThreeFruitPoison();
                break;
            case "enemy06":
                //enemyAction = new CreateWaterInBox();
                break;
            case "enemy07":
                enemyAction = new GetPoison();
                break;
            case "enemy08":
                enemyAction = new GetGasPoison();
                break;
            case "enemy09":
                enemyAction = new DropThreeFruitThorn();
                break;
            case "enemy10":
                break;
            case "enemy11":
                break;
            case "enemy12":
                break;
            case "enemy13":
                break;
            case "enemy14":
                break;
            case "enemy15":
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
