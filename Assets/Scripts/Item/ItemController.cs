using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : Singleton<ItemController>
{
    [SerializeField] private List<GameObject> listPosSpawnItem;
    [SerializeField] private ItemPrefabs currentObjectPrefab;
    [SerializeField] private Transform itemParent;

    [Space]
    [Header("Item")]
    [SerializeField] private List<ItemPrefabs> listItemInBox;
    [SerializeField] private List<ItemPrefabs> listItemInBasket;

    [Space]
    [Header("Data")]
    [SerializeField] private ItemData data;

    public void Init(ItemPrefabs item)
    {
        int randomNumber = Random.Range(0, data.listDataItem.Count);  //random du lieu

        //Init
        DataItem dataItem = data.listDataItem[randomNumber];
        item.Init(dataItem.id, dataItem.name, dataItem.icon);
    }

    public void Spawn()
    {
        Debug.Log("Spawn Iten");
        for (int i = 0; i < listPosSpawnItem.Count; i++)
        {
            ItemPrefabs item = PoolingManager.Spawn(currentObjectPrefab, listPosSpawnItem[i].transform.position, Quaternion.identity, itemParent);
            Init(item);
            listItemInBox.Add(item);
            item.gameObject.SetActive(true);
        }
    }

    public void ChangeBoxToBasket(ItemPrefabs item)
    {
        if(listItemInBox.Contains(item))
            listItemInBox.Remove(item);

        if(!listItemInBasket.Contains(item))
            listItemInBasket.Add(item);
    }
    public void DeleteItemOutBasket(ItemPrefabs item)
    {
        listItemInBasket.Remove(item);
    }
    public void CheckNextTurn()
    {
        if (listItemInBasket.Count == 0)
            GameController.Instance.isCheckTurnByItem = true;
        else if (listItemInBasket.Count > 0)
            GameController.Instance.isCheckTurnByItem = false;
    }
}
