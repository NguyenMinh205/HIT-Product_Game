using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionDrop : MonoBehaviour
{

}

public class DropThreeFruitPoison : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        GamePlayController.Instance.ItemController.DropInBox(enemy.ItemEffect, 3);
    }
}

public class DropThreeHoney : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        GamePlayController.Instance.ItemController.DropInBox(enemy.ItemEffect, 3);
    }
}
public class DropThreeFruitThorn : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        GamePlayController.Instance.ItemController.DropInBox(enemy.ItemEffect, 3);
    }
}
public class DropFluffInBox : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        int val = Random.Range(5, 10);
        GamePlayController.Instance.ItemController.DropInBox(enemy.ItemEffect, val);
    }
}
public class DropThreeFruitPosionThorn : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        GamePlayController.Instance.ItemController.DropInBox(enemy.ItemEffect, 3);
    }
}
