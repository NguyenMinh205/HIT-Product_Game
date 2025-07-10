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
    [SerializeField] private List<Enemy> listEnemy;

    [Space]
    [Header("List Position Spawn Enemy")]
    [SerializeField] private List<Transform> listPosSpawnEnemy;

    [Space]
    [Header("List Spawn Controller")]
    [SerializeField] private EnemySpawController enemySpawController;

    public List<Enemy> ListEnemy
    {
        get => listEnemy;
    }
    private void Awake()
    {
        ObserverManager<IDEnemyState>.AddDesgisterEvent(IDEnemyState.EnemyDied, DieEnemy);
    }
    public void Spawn()
    {
        SpawnEnemy(enemySpawController.GetListEnemyToSpawn(MapManager.Instance.MapIndex));
    }
    public void SpawnEnemy(List<string> dataEnemySetUp)
    {
        Debug.Log("Spawn Enemy");
        for(int i=0; i<dataEnemySetUp.Count; i++)
        {
            //Spawn New Enemy Base
            Enemy newEnemy = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[i].position, Quaternion.identity, enemyParent);

            newEnemy.Init(enemyData, dataEnemySetUp[i]);
            newEnemy.CalulationPositionEnemy(listPosSpawnEnemy[i].position);

            //Add New Enemy To List
            listEnemy.Add(newEnemy);
        }
    }
    public IEnumerator CheckEnemyToNextTurn()
    {
        for (int i= 0; i < listEnemy.Count ;i++)
        {
            yield return new WaitForSeconds(2f);
            listEnemy[i].ExecuteAction();
        }
        GamePlayController.Instance.Turn = TurnPlay.Player;
        for (int i = 0; i < listEnemy.Count; i++)
        {
            listEnemy[i].NextAction();
        }
    }
    public void DieEnemy(object obj)
    {
        if(obj is Enemy enemy)
        {
            Debug.Log("Die Enemy");
            if(listEnemy.Contains(enemy))
            {
                listEnemy.Remove(enemy);
            }
            enemy.UIAction.UnShowActionEnemy();
            enemy.Health.UnShowHealthBarEnemy();
            PoolingManager.Despawn(enemy.gameObject);
        }
    }
    public void EndGame()
    {
        Debug.Log("Enemy Count : " + listEnemy.Count);
        for(int i=0; i < listEnemy.Count; i++)
        {
            Debug.Log("DesTroy Enemy" + i);
            //listEnemy[i].EndGame();
        }
        listEnemy.Clear();
    }

}
