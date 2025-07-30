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
    [SerializeField] private ItemMoveController move;

    [Space]
    [Header("Item Lists")]
    [SerializeField] private List<Item> listItemInBox;
    public List<Item> ListItemInBox => listItemInBox;
    [SerializeField] private List<Item> listItemInBasket;
    [SerializeField] private List<int> listIndexUsed = new List<int>();
    public bool IsPickupItemSuccess { get; set; }

    [Space]
    [Header("Item Effect")]
    [SerializeField] private ItemBase thorn;

    private void Awake()
    {
        ObserverManager<IDItem>.AddDesgisterEvent(IDItem.ItemChange, ChangeBoxToBasket);
        ObserverManager<IDItem>.AddDesgisterEvent(IDItem.ItemPlayer, DeleteItemOutBasket);
    }

    public void Spawn(Inventory inven)
    {
        SpawnItem(inven.Items);
        listItemInBasket.Clear();
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

    public void ChangeBoxToBasket(object obj)
    {
        if (obj is Item item)
        {
            if (listItemInBox.Contains(item))
            {
                IsPickupItemSuccess = true;
                listItemInBox.Remove(item);
                listItemInBasket.Add(item);
                GamePlayController.Instance.PlayerController.CurrentPlayer.RemoveItem(item.ItemBase.id, isUpgraded: item.ItemBase.isUpgraded);
                Debug.Log($"Item {item.ID} moved to basket and removed from inventory");
            }
        }
    }

    public void DeleteItemOutBasket(object obj)
    {
        if (obj is Item item)
        {
            Debug.Log($"Deleting item {item.ID} from basket");
            listItemInBasket.Remove(item);
            if (listItemInBasket.Count == 0)
            {
                ObserverManager<EventID>.PostEven(EventID.OnBasketEmpty);
                Debug.Log("Basket is empty, notifying turn change.");
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
        listItemInBasket.Clear();
        move.EndGame();
        Debug.Log("Cleared all items from machine");
    }

    public void SpawnAdditionalItems()
    {
        var addedItems = GamePlayController.Instance.PlayerController.CurrentPlayer.AddedItems;
        if (addedItems.Count > 0)
        {
            SpawnItem(addedItems);
            foreach (var item in addedItems)
            {
                Debug.Log($"Spawned additional item {item.itemId} (isUpgraded: {item.isUpgraded}) with quantity {item.quantity} for new turn");
            }
        }
        else
        {
            Debug.Log("No items in addedItems to spawn");
        }
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