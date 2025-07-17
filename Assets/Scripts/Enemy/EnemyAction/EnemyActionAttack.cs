using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionAttack : MonoBehaviour
{
    public static void Execute(Enemy enemy)
    {
        GamePlayController.Instance.PlayerController.CurrentPlayer.ReceiveDamage(enemy.Damage);
    }
}
