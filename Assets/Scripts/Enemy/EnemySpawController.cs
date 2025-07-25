using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawController : MonoBehaviour
{
    private List<string> listIDEnemy = new List<string> { "enemy01", "enemy02", "enemy03", "enemy04", "enemy05", "enemy06", "enemy07", "enemy08", "enemy09", "enemy10", "enemy11"
    , "enemy12", "enemy13", "enemy15", "enemy16", "enemy17", "enemy18", "enemy21", "enemy22"};

    private List<string> listIDEnemyApt = new List<string>();
    private int currentIndexFloor = 0;

    private List<string> listIDBoss = new List<string> {"boss02","boss06" };    
    private int currentIndexBoss = 0;

    public void SetIDEnemyApt(int indexFloor)
    {
        if(indexFloor == 1 && currentIndexFloor == 0)
        {
            currentIndexFloor = 1;
            for(int i=0;i<5;i++)
            {
                listIDEnemyApt.Add(listIDEnemy[i]);
            }
        }
        else if(currentIndexFloor < indexFloor)
        {
            listIDEnemyApt.Add(listIDEnemy[currentIndexFloor+4]);
            currentIndexFloor++;
            listIDEnemyApt.RemoveAt(0);
        }
    }
    public List<string> GetListEnemyToSpawn(int indexFloor)
    {
        SetIDEnemyApt(indexFloor);

        List<string> listEnemySpawn = new List<string>();

        int enemyCount = Random.Range(2, 4);     // random enemy count
        
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0, listIDEnemyApt.Count);
            
            listEnemySpawn.Add(listIDEnemyApt[randomIndex]);
        }
        //test enemy
        //return new List<string> { "enemy20" };
        return listEnemySpawn;
    }

    public string GetIDBossToSpawn()
    {
        if (currentIndexBoss >= listIDBoss.Count)
        {
            currentIndexBoss = 0;
        }
        string idBoss = listIDBoss[currentIndexBoss];
        currentIndexBoss++;
        return idBoss;
        //return "boss06";
    }
}
