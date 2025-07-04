using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionDefend : MonoBehaviour
{
    public static void Execute(Enemy enemy)
    {
        enemy.Armor += enemy.ArmorIncreased;
        enemy.Health.UpdateArmor(enemy);
    }
}
