using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> listObject;
    [SerializeField] private GameObject currentObjectPrefab;

    private void Start()
    {
        Spawn();
    }
    private void Update()
    {
        CheckSpawn();
    }
    public void CheckSpawn()
    {
        if (GameController.Instance.Turn == TurnPlay.Player && GameController.Instance.IsChange02)
        {
            Spawn();
            GameController.Instance.IsChange02 = false;
        }
    }
    public void Spawn()
    {
            for(int i = 0; i < listObject.Count; i++)
            {
                GameObject item = PoolingManager.Spawn(currentObjectPrefab, listObject[i].transform.position, Quaternion.identity);
                item.SetActive(true);
            }
            GameController.Instance.IsChange02 = false;
    }
}


