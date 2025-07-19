using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<ItemInventory> items = new List<ItemInventory>();
    public List<ItemInventory> Items => items;

    public void AddItem(string itemId, int quantity, int maxQuantity = 99, bool isUpgraded = false)
    {
        ItemInventory existingItem = items.Find(item => item.itemId == itemId && item.isUpgraded == isUpgraded);
        if (existingItem != null)
        {
            existingItem.quantity = Mathf.Min(existingItem.quantity + quantity, maxQuantity);
        }
        else
        {
            ItemInventory newItem = new ItemInventory(itemId, quantity, isUpgraded);
            items.Add(newItem);
        }
        UpdateInventoryUI();
        GamePlayController.Instance.PlayerController.SavePlayerData();
    }

    public void RemoveItem(string itemId, int quantity, bool isUpgraded = false)
    {
        ItemInventory item = items.Find(item => item.itemId == itemId && item.isUpgraded == isUpgraded);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                items.Remove(item);
            }
            UpdateInventoryUI();
            GamePlayController.Instance.PlayerController.SavePlayerData();
        }
    }

    public void UpgradeItem(ItemInventory item)
    {
        if (item != null && item.CanUpgrade)
        {
            ItemBase currentItem = ItemDatabase.Instance.GetItemById(item.itemId);
            if (currentItem != null && currentItem.upgradedItem != null)
            {
                item.quantity--;
                if (item.quantity <= 0)
                {
                    items.Remove(item);
                }
                AddItem(currentItem.id, 1, 99, true);
                UpdateInventoryUI();
                GamePlayController.Instance.PlayerController.SavePlayerData();
            }
        }
    }

    public List<ItemInventory> GetItems()
    {
        return new List<ItemInventory>(items);
    }

    public void ClearInventory()
    {
        items.Clear();
        UpdateInventoryUI();
        GamePlayController.Instance.PlayerController.SavePlayerData();
    }

    private void UpdateInventoryUI()
    {
        // Implementation...
    }
}