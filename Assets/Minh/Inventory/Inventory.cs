using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemInventory> items = new List<ItemInventory>();
    public List<ItemInventory> Items => items;

    public void AddItem(ItemBase itemBase, int quantity, int maxQuantity = 99)
    {
        ItemInventory existingItem = items.Find(item => item.ItemId == itemBase.id);

        if (existingItem != null)
        {
            if (existingItem.quantity == maxQuantity) return;
            existingItem.quantity += quantity;
            if (existingItem.quantity > maxQuantity) existingItem.quantity = maxQuantity;
        }
        else
        {
            items.Add(new ItemInventory(itemBase, quantity));
        }
        UpdateInventoryUI();
    }

    public void RemoveItem(ItemBase itemBase, int quantity)
    {
        ItemInventory item = items.Find(item => item.itemBase == itemBase);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                items.Remove(item);
            }
            UpdateInventoryUI();
        }
    }

    public void UpgradeItem(ItemInventory item)
    {
        if (item != null && item.CanUpgrade)
        {
            // Thay thế ItemBase bằng upgradedItem
            ItemBase upgradedItemBase = item.itemBase.upgradedItem;
            item.quantity--; // Giảm số lượng vật phẩm gốc
            if (item.quantity <= 0)
            {
                items.Remove(item);
            }
            // Thêm vật phẩm nâng cấp với số lượng 1
            ItemInventory newItem = new ItemInventory(upgradedItemBase, 1);
            items.Add(newItem);
            UpdateInventoryUI();
        }
    }

    public List<ItemInventory> GetItems()
    {
        return new List<ItemInventory>(items);
    }

    public void ClearInventory()
    {
        items.Clear();
    }    

    private void UpdateInventoryUI()
    {
        //if (inventoryUIController != null)
        //{
        //    inventoryUIController.UpdateInventoryDisplay(items);
        //}
    }
}