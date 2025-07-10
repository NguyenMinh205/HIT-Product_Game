using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionEffect : MonoBehaviour
{
    
}

public class DropThreeFruitPoison : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}

public class DropThreeHoney : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}

/*public class CreateWaterInBox : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}*/

public class GetPoison : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        //GamePlayController.Instance.playerController.CurrentPlayer.IsPosion = true;
       // GamePlayController.Instance.playerController.CurrentPlayer.PoisonDamage += 10;
    }
}
public class GetGasPoison : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
       
    }
}

public class DropThreeFruitThorn : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
       
    }
}
public class DropFluffInBox : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
       
    }
}
public class DropThreeFruitPosionThorn : IEnemyAction
{
    public void Execute(Enemy enemy)
    {
        
    }
}