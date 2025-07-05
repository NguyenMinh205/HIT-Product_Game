using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    [SerializeField] private List<GameObject> listPosSpawnItem;
    [SerializeField] private Item currentObjectPrefab;
    [SerializeField] private Transform itemParent;

    [Space]
    [Header("Item")]
    [SerializeField] private List<Item> listItemInBox;
    [SerializeField] private List<Item> listItemInBasket;

    [Space]
    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        
    }


    public void Spawn(Inventory inven)
    {
        SpawnItem(inven.Items);
    }
    public void SpawnItem(List<ItemInventory> items)
    {
        Debug.Log("Spawn Item");

        foreach(ItemInventory item in items)  
        {
            int qualityItem = item.quantity; // so luong item
            //Tinh toan so luong Item can Spawm


            //---------
            int coutItem = qualityItem;

            for(int i=0;i<coutItem;i++)
            {
                Item newItem = PoolingManager.Spawn(currentObjectPrefab, listPosSpawnItem[i].transform.position, Quaternion.identity, itemParent);

                newItem.Init(item.itemBase);
                listItemInBox.Add(newItem);
                newItem.gameObject.SetActive(true);
            }
            
        }
    }

    public void ChangeBoxToBasket(Item item)
    {
        if(listItemInBox.Contains(item))
            listItemInBox.Remove(item);

        if(!listItemInBasket.Contains(item))
            listItemInBasket.Add(item);
    }
    public void DeleteItemOutBasket(Item item)
    {
        listItemInBasket.Remove(item);
    }
    public void CheckNextTurn()
    {
        /*if (listItemInBasket.Count == 0)
            //GameController.Instance.isCheckTurnByItem = true;
        else if (listItemInBasket.Count > 0)
            //GameController.Instance.isCheckTurnByItem = false;*/
    }

    public void EndGame()
    {
        for(int i=0; i < listItemInBox.Count; i++)
        {
            PoolingManager.Despawn(listItemInBox[i].gameObject);
        }
        listItemInBox.Clear();
    }
}
