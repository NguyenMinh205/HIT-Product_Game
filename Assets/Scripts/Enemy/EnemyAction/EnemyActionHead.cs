using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionHead : MonoBehaviour
{
    private int recoveruHP = 15;
    public static void Execute(Enemy enemy)
    {
        if(CheckEnemy(enemy))
        {
            return;
        }
        else
        {
            foreach (Enemy var in GamePlayController.Instance.enemyController.ListEnemy)
            {
                if (CheckEnemy(var))
                    return;
            }
        }
    }

    public static bool CheckEnemy(Enemy enemy)
    {
        if (enemy.HP < enemy.CurrentHp)
        {
            int recovery = enemy.HP + 15;
            enemy.HP = Mathf.Min(enemy.CurrentHp, recovery);
            return true;
        }

        return false;
    }
}
