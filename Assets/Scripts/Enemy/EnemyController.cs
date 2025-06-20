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

    public List<Enemy> ListEnemy
    {
        get => listenemy;
    }
    public void SpawnEnemy()
    {
        Debug.Log("Spawn ENemy");
        for(int i=0;i<enemys;i++)
        {
            Enemy newEnemy = PoolingManager.Spawn(enemyPrefab, listPosSpawnEnemy[i].position, Quaternion.identity, enemyParent);
            newEnemy.HP = newEnemy.CurrentHp;
            newEnemy.Controller = this;
            listenemy.Add(newEnemy);
            ObserverManager<IDInfoObject>.PostEven(IDInfoObject.ShowInfo, listenemy[i]);
            newEnemy.InitActionManager();
        }
    }
    public void InitActionListEnemy()
    {
        for (int i = 0; i < listenemy.Count; i++)
        {
            listenemy[i].InitActionManager();
        }
    }
    public IEnumerator CheckEnemyToNextTurn()
    {
        for (int i= 0; i < listenemy.Count ;i++)
        {
            yield return new WaitForSeconds(2f);
            listenemy[i].ExecuteAction();
        }
        GameController.Instance.Turn = TurnPlay.Player;
    }
    public void DieEnemy()
    {
        listenemy.RemoveAt(0);
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
