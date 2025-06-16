using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private List<GameObject> listPosSpawnItem;
    [SerializeField] private GameObject currentObjectPrefab;
    [SerializeField] private Transform itemParent;

    [Space]
    [Header("Item")]
    [SerializeField] private List<GameObject> listItemInBox;
    [SerializeField] private List<GameObject> listItemInBasket; 

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
        for (int i = 0; i < listPosSpawnItem.Count; i++)
        {
            GameObject item = PoolingManager.Spawn(currentObjectPrefab, listPosSpawnItem[i].transform.position, Quaternion.identity, itemParent);
            listItemInBox.Add(item);
            item.SetActive(true);
        }
    }
    public void CheckItem()
    {

    }
}
