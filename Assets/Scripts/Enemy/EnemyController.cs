using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Data Enemy")]
    [SerializeField] private EnemyData enemyData;

    [Space]
    [Header("Enemy Object")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyParent;

    [Space]
    [Header("List Enemy")]
    [SerializeField] private List<Enemy> listenemy;

    [Space]
    [Header("List Position Spawn Enemy")]
    [SerializeField] private List<Transform> listPosSpawnEnemy;

    public List<Enemy> ListEnemy
    {
        get => listenemy;
    }
    private void Awake()
    {
        ObserverManager<IDEnemyState>.AddDesgisterEvent(IDEnemyState.EnemyDied, DieEnemy);
    }
    public void SpawnEnemy(List<string> dataEnemySetUp)
    {
        Debug.Log("Spawn Enemy");
        for(int i=0; i<dataEnemySetUp.Count; i++)
        {
            //Spawn New Nnemy Base
            Enemy newEnemy = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[i].position, Quaternion.identity, enemyParent);

            newEnemy.Init(enemyData, dataEnemySetUp[i]);
            newEnemy.CalulationPositionEnemy(listPosSpawnEnemy[i].position);

            //Add New Enemy To List
            listenemy.Add(newEnemy);
        }
    }
    public IEnumerator CheckEnemyToNextTurn()
    {
        checkPosionEnemy();
        for (int i= 0; i < listenemy.Count ;i++)
        {
            yield return new WaitForSeconds(2f);
            listenemy[i].ExecuteAction();
        }
        GameController.Instance.Turn = TurnPlay.Player;
        for (int i = 0; i < listenemy.Count; i++)
        {
            listenemy[i].NextAction();
        }
    }
    public void DieEnemy(object obj)
    {
        if(obj is Enemy enemy)
        {
            Debug.Log("Die Enemy");
            if(listenemy.Contains(enemy))
            {
                listenemy.Remove(enemy);
            }
            enemy.UIAction.UnShowActionEnemy();
            enemy.Health.UnShowHealthBarEnemy();
            PoolingManager.Despawn(enemy.gameObject);
        }
    }
    public void checkPosionEnemy()
    {
        for (int i = 0; i < listenemy.Count; i++)
        {
            listenemy[i].CheckIsPoison();
        }
    }
    public void EndGame()
    {
        Debug.Log("Enemy Count : " + listenemy.Count);
        for(int i=0; i < listenemy.Count; i++)
        {
            Debug.Log("DesTroy Enemy" + i);
            listenemy[i].EndGame();
        }
        listenemy.Clear();
    }

}
