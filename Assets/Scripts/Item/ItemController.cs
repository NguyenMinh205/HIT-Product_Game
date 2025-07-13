using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDItem
{
    ItemChange,
    ItemPlayer,
}

public class ItemController : MonoBehaviour
{
    [SerializeField] private List<GameObject> listPosSpawnItem; // Danh sách vị trí spawn vật phẩm
    [SerializeField] private Item currentObjectPrefab; // Prefab vật phẩm
    [SerializeField] private Transform itemParent; // Parent cho các vật phẩm được spawn

    [Space]
    [Header("Item Lists")]
    [SerializeField] private List<Item> listItemInBox; // Danh sách vật phẩm trong máy gắp
    [SerializeField] private List<Item> listItemInBasket; // Danh sách vật phẩm trong giỏ

    private void Awake()
    {
        // Đăng ký sự kiện cho Observer
        ObserverManager<IDItem>.AddDesgisterEvent(IDItem.ItemChange, ChangeBoxToBasket);
        ObserverManager<IDItem>.AddDesgisterEvent(IDItem.ItemPlayer, DeleteItemOutBasket);
    }

    private void Update()
    {
        CheckNextTurn();
    }

    // Spawn vật phẩm từ inventory của Player khi vào room
    public void Spawn(Inventory inven)
    {
        SpawnItem(inven.Items);
    }

    public void SpawnItem(List<ItemInventory> items)
    {
        Debug.Log("Spawning items in machine");

        foreach (ItemInventory item in items)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                int randomIndex = Random.Range(0, listPosSpawnItem.Count);
                Vector3 spawnPos = listPosSpawnItem[randomIndex].transform.position;

                Item newItem = PoolingManager.Spawn(currentObjectPrefab, spawnPos, Quaternion.identity, itemParent);
                newItem.Init(item.itemBase);
                listItemInBox.Add(newItem);
                newItem.gameObject.SetActive(true);
            }
        }
    }

    public void ChangeBoxToBasket(object obj)
    {
        if (obj is Item item)
        {
            if (listItemInBox.Contains(item))
            {
                listItemInBox.Remove(item);
                listItemInBasket.Add(item);

                GamePlayController.Instance.PlayerController.CurrentPlayer.RemoveItem(item.ItemBase);
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
        }
    }

    public void CheckNextTurn()
    {
        GamePlayController.Instance.isCheckTurnByItem = listItemInBasket.Count == 0;
    }

    public void EndGame()
    {
        foreach (Item item in listItemInBox)
        {
            PoolingManager.Despawn(item.gameObject);
        }
        listItemInBox.Clear();
        listItemInBasket.Clear();
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
                Debug.Log($"Spawned additional item {item.itemBase.id} with quantity {item.quantity} for new turn");
            }
        }
        else
        {
            Debug.Log("No items in addedItems to spawn");
        }
    }
}