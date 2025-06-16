using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyParent;

    [SerializeField] private int enemys;

    [SerializeField] private List<Enemy> listenemy;

    [SerializeField] private List<Transform> listPosSpawnEnemy;

    public void SpawnEnemy()
    {
        for(int i=0;i<enemys;i++)
        {
            Enemy newEnemy = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[i].position, Quaternion.identity, enemyParent);
            HpController.Instance.showHp(i + 1);
            listenemy.Add(newEnemy);
        }
    }

    public void CheckEnemyToNextTurn()
    {
        for (int i= 0; i < listenemy.Count ;i++)
        {
            listenemy[i].Attack(GameController.Instance.Player.gameObject);
        }
    }

    public void DieEnemy()
    {
        listenemy.RemoveAt(0);
    }
}
