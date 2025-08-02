using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Data Enemy")]
    [SerializeField] private EnemyData enemyData;

    [Space]
    [Header("Enemy Object")]
    [SerializeField] private GameObject enemyPrefab;
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
    public void Spawn()
    {
        SpawnEnemy(enemySpawController.GetListEnemyToSpawn(MapSystem.Instance.MapIndex));
    }
    public void SetPosEnemy(GameObject currentRoom, string checkRoom)
    {
        listPosSpawnEnemy.Clear();
        if(checkRoom == "BossRoom")
        {
            listPosSpawnEnemy.Add(currentRoom.transform.Find("BossPos"));
        }
        else
        {
            listPosSpawnEnemy.Add(currentRoom.transform.Find("EnemyPos 1"));
            listPosSpawnEnemy.Add(currentRoom.transform.Find("EnemyPos 2"));
            listPosSpawnEnemy.Add(currentRoom.transform.Find("EnemyPos 3"));
        }
    }
    public void SpawnEnemy(List<string> dataEnemySetUp)
    {
        if (GamePlayController.Instance.IsEndGame)
        {
            return;
        }
        for (int i = 0; i < dataEnemySetUp.Count; i++)
        {
            GameObject newObject = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[i].position, Quaternion.identity, enemyParent);
            Enemy newEnemy = newObject.GetComponentInChildren<Enemy>();
            newEnemy.InitEnemy(enemyData, dataEnemySetUp[i]);
            listEnemy.Add(newEnemy);
        }
    }
    public void SpawnBoss()
    {
        string boosID = enemySpawController.GetIDBossToSpawn();
        if (GamePlayController.Instance.IsEndGame)
        {
            return;
        }
        GameObject newObject = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[0].position, Quaternion.identity, enemyParent);
        Enemy newEnemy = newObject.transform.Find("EnemyPrefab").GetComponent<Enemy>();
        newEnemy.InitBoss(enemyData, boosID);
        listEnemy.Add(newEnemy);
    }
    public IEnumerator CheckEnemyToNextTurn()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            StartCoroutine(listEnemy[i].ExecuteAction());
            float time = listEnemy[i].actions[listEnemy[i].IndexAction].actionEnemy.Count;

            if(listEnemy[i].actions[listEnemy[i].IndexAction].actionEnemy.Count > 3)
            {
                time += 0.1f;
            }

            Debug.Log("Time Enemy: " + time);
            yield return new WaitForSeconds(time);
        }
        yield return new WaitForSeconds(0.5f);
        GamePlayController.Instance.Turn = TurnPlay.Player;
        for (int i = 0; i < listEnemy.Count; i++)
        {
            Debug.Log("Next Action Enemy:  " + listEnemy[i].IndexAction);
            listEnemy[i].NextAction();
        }
    }

    public void DieEnemy(Enemy enemy)
    {
        Debug.Log("Die Enemy");
        if (listEnemy.Contains(enemy))
        {
            listEnemy.Remove(enemy);
            GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(1);
            CheckEnemyCountZero();
            enemy.UIAction.UnShowActionEnemy();
            enemy.Health.UnShowHealthBarEnemy();
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