using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawController : MonoBehaviour
{
    private List<string> listIDEnemy = new List<string> { "enemy01", "enemy02", "enemy03", "enemy04", "enemy05", "enemy06", "enemy07", "enemy08", "enemy09", "enemy10", "enemy11",
        "enemy12", "enemy13", "enemy15", "enemy16", "enemy17", "enemy18", "enemy21", "enemy22" };

    private const int numOfEnemyInFloor = 5;
    private List<string> listIDBoss = new List<string> { "boss01", "boss02", "boss03", "boss04", "boss05", "boss06", "boss07" };

    private List<string> availableBossIDs;

    private void Start()
    {
        availableBossIDs = new List<string>(listIDBoss);
        foreach (string usedBoss in GameData.Instance.mainGameData.usedBossIDs)
        {
            availableBossIDs.Remove(usedBoss);
        }
    }

    public List<string> GetEnemyListForFloor(int floor)
    {
        List<string> enemyList = new List<string>();
        int startIndex, endIndex;

        if (floor == 1)
        {
            startIndex = 0;
            endIndex = numOfEnemyInFloor - 1;
        }
        else if (floor == 2)
        {
            startIndex = 0;
            endIndex = 6;
        }
        else
        {
            int cycle = (floor - 3) / 3;
            int phase = (floor - 3) % 3;

            if (phase == 0)
            {
                startIndex = 2 * cycle + 2;
                endIndex = startIndex + numOfEnemyInFloor - 1;
            }
            else if (phase == 1)
            {
                startIndex = 2 * cycle + 2;
                endIndex = startIndex + 6;
            }
            else
            {
                startIndex = 2 * cycle + 4;
                endIndex = startIndex + numOfEnemyInFloor - 1;
            }
        }

        for (int i = startIndex; i <= endIndex && i < listIDEnemy.Count; i++)
        {
            enemyList.Add(listIDEnemy[i]);
        }

        while (enemyList.Count < numOfEnemyInFloor && listIDEnemy.Count > 0)
        {
            enemyList.Add(listIDEnemy[listIDEnemy.Count - 1]);
        }

        return enemyList;
    }

    public List<string> GetListEnemyToSpawn(int indexFloor)
    {
        List<string> floorEnemyList = GetEnemyListForFloor(indexFloor);
        List<string> listEnemySpawn = new List<string>();
        int enemyCount = Random.Range(2, 4);
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0, floorEnemyList.Count);
            listEnemySpawn.Add(floorEnemyList[randomIndex]);
        }

        return listEnemySpawn;
    }

    public string GetIDBossToSpawn()
    {
        //int numFloor = MapManager.Instance.NumFloor;
        //if (GameData.Instance.mainGameData.curFloor == numFloor)
        //{
        //    if (!GameData.Instance.mainGameData.usedBossIDs.Contains("boss07"))
        //    {
        //        GameData.Instance.mainGameData.usedBossIDs.Add("boss07");
        //        GameData.Instance.SaveMainGameData();
        //    }
        //    return "boss07";
        //}

        //if (availableBossIDs.Count == 0)
        //{
        //    availableBossIDs = new List<string>(listIDBoss);
        //    availableBossIDs.Remove("boss07");
        //}

        //int randomIndex = Random.Range(0, availableBossIDs.Count);
        //string idBoss = availableBossIDs[randomIndex];
        //availableBossIDs.RemoveAt(randomIndex);
        //GameData.Instance.mainGameData.usedBossIDs.Add(idBoss);
        //GameData.Instance.SaveMainGameData();

        //return idBoss;
        return "boss02";
    }
}