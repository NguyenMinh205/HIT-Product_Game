using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDItem
{
    ItemChange,
    ItemPlayer
}

public class ItemController : MonoBehaviour
{
    [SerializeField] private List<GameObject> listPosSpawnItem;
    [SerializeField] private Item currentObjectPrefab;
    [SerializeField] private Transform itemParent;


    [Space]
    [Header("Item Lists")]
    [SerializeField] private List<Item> listItemInBox;
    public List<Item> ListItemInBox => listItemInBox;
    [SerializeField] private List<int> listIndexUsed = new List<int>();
    public bool IsPickupItemSuccess { get; set; }
    private bool isItemNull = true;
    public bool IsItemNull => isItemNull;

    [Space]
    [Header("Item Effect")]
    [SerializeField] private ItemBase thorn;

    private void Awake()
    {
        ObserverManager<IDItem>.AddDesgisterEvent(IDItem.ItemChange, RemoveItemBox);
    }
    private void OnDisable()
    {
        ObserverManager<IDItem>.RemoveAddListener(IDItem.ItemChange, RemoveItemBox);
    }

    public void Spawn(Inventory inven)
    {
        SpawnItem(inven.Items);
    }

    public void SpawnItem(List<ItemInventory> items)
    {
        if (GamePlayController.Instance.IsEndGame)
        {
            return;
        }

        listIndexUsed.Clear();

        foreach (ItemInventory item in items)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                int randomIndex = Random.Range(0, listPosSpawnItem.Count);

                while (listIndexUsed.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, listPosSpawnItem.Count);
                }

                AddListCheck(randomIndex);
                float randomY = Random.Range(-0.5f, 1f);

                Vector3 spawnPos = listPosSpawnItem[randomIndex].transform.position;
                spawnPos.y += randomY; 
                Item newItem = PoolingManager.Spawn(currentObjectPrefab, spawnPos, Quaternion.identity, itemParent);
                ItemBase itemBase = item.GetItemBase();

                if (itemBase != null)
                {
                    newItem.Init(itemBase);
                    listItemInBox.Add(newItem);
                    newItem.gameObject.SetActive(true);
                }

            }
        }
    }
    public void AddListCheck(int index)
    {
        listIndexUsed.Add(index);
        if(listIndexUsed.Count >= listPosSpawnItem.Count)
        {
            listIndexUsed.Clear();
        }
    }

    public void DropInBox(ItemBase item, int val)
    {
        Debug.Log("Enemy Drop Item Effect In Box"); 
        int value = val;

        listIndexUsed.Clear();
        while (value > 0)
        {
            int posSpawn = Random.Range(0, listPosSpawnItem.Count);

            while (listIndexUsed.Contains(posSpawn))
            {
                posSpawn = Random.Range(0, listPosSpawnItem.Count);
            }

            AddListCheck(posSpawn);
            Vector3 spawnPos = listPosSpawnItem[posSpawn].transform.position;
            Item newItem = PoolingManager.Spawn(currentObjectPrefab, spawnPos, Quaternion.identity, itemParent);
            if (newItem != null)
            {
                newItem.Init(item);
                listItemInBox.Add(newItem);
                newItem.gameObject.SetActive(true);
                Debug.Log($"Spawned item {item.id} at position {spawnPos}");
            }
            else
            {
                Debug.LogError("Failed to spawn item, newItem is null");
            }
            value--;
        }
    }

    public void RemoveItemBox(object obj)
    {
        if (obj is Item item)
        {
            if (listItemInBox.Contains(item))
            {
                listItemInBox.Remove(item);
                ItemTube.Instance.AddItem(item);
                GamePlayController.Instance.PlayerController.CurrentPlayer.RemoveItem(item.ItemBase.id, isUpgraded: item.ItemBase.isUpgraded);
                PoolingManager.Despawn(item.gameObject);
            }
        }
    }


    public void EndGame()
    {
        foreach (Item item in listItemInBox)
        {
            PoolingManager.Despawn(item.gameObject);
        }
        listItemInBox.Clear();

        Debug.Log("Cleared all items from machine");
    }

    public void ChangeToThorn(int val)
    {
        int check = 0;
        foreach (Item item in listItemInBox)
        {
            if(check < val)
            {
                item.Init(thorn);
                check++;
            }
            else
            {
                return;
            }

        }
    }
}