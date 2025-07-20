using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionEffect : MonoBehaviour
{
    
}

public class CreateWaterInBoxEffect : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        Debug.Log("Create Water In Box Effect");
        enemy.AddBuffEffect("create_water_in_box", 0, 3);
    }
}

