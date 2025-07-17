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
        if (GamePlayController.Instance.IsEndGame)
        {
            return;
        }
        for (int i = 0; i < dataEnemySetUp.Count; i++)
        {
            Enemy newEnemy = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[i].position, Quaternion.identity, enemyParent);
            newEnemy.Init(enemyData, dataEnemySetUp[i]);
            listEnemy.Add(newEnemy);
        }
    }

    public IEnumerator CheckEnemyToNextTurn()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            StartCoroutine(listEnemy[i].ExecuteAction());
            float time = listEnemy[i].actions[listEnemy[i].IndexAction].actionEnemy.Count;
            yield return new WaitForSeconds(time);
        }
        GamePlayController.Instance.Turn = TurnPlay.Player;
        for (int i = 0; i < listEnemy.Count; i++)
        {
            listEnemy[i].NextAction();
        }
    }

    public void DieEnemy(object obj)
    {
        if (obj is Enemy enemy)
        {
            Debug.Log("Die Enemy");
            if (listEnemy.Contains(enemy))
            {
                listEnemy.Remove(enemy);
                GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(1);
                CheckEnemyCountZero();
            }
            enemy.UIAction.UnShowActionEnemy();
            enemy.Health.UnShowHealthBarEnemy();
            PoolingManager.Despawn(enemy.gameObject);
        }
    }

    public void CheckEnemyCountZero()
    {
        Debug.Log("Enemy Count is Zero");
        if (listEnemy.Count == 0)
            GamePlayController.Instance.WinGame();
    }

    public void ResetShield()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            Debug.Log("Reset Shield Enemy" + i);
            listEnemy[i].Armor = 0;
            listEnemy[i].Health.UpdateArmor(listEnemy[i]);
        }
    }

    public void EndGame()
    {
        listEnemy.Clear();
    }
}