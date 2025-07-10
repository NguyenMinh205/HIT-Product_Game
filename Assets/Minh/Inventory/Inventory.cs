using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemInventory> items = new List<ItemInventory>();
    public List<ItemInventory> Items => items;

    public void AddItem(ItemBase itemBase, int quantity, int maxQuantity = int.MaxValue)
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

    public void RemoveItem(string itemId, int quantity)
    {
        ItemInventory item = items.Find(item => item.ItemId == itemId);
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

    public List<ItemInventory> GetItems()
    {
        return new List<ItemInventory>(items);
    }

    private void UpdateInventoryUI()
    {
        //if (inventoryUIController != null)
        //{
        //    inventoryUIController.UpdateInventoryDisplay(items);
        //}
    }

    //[SerializeField] private InventoryUIController inventoryUIController;
}