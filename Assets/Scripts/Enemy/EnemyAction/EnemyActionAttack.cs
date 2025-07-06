using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionAttack : MonoBehaviour
{
    public static void Execute(Enemy enemy)
    {
        GamePlayController.Instance.playerController.CurrentPlayer.Stats.currentHP -= enemy.Damage;
        GamePlayController.Instance.playerController.CurrentPlayer.Health.UpdateHp(GamePlayController.Instance.playerController.CurrentPlayer);
    }
}
