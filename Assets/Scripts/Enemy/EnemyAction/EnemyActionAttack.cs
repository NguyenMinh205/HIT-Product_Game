using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionAttack : MonoBehaviour
{
    public static void Execute(Enemy enemy)
    {
        GameController.Instance.playerController.CurrentPlayer.ReceiverDamage(enemy.Damage);
    }
}
