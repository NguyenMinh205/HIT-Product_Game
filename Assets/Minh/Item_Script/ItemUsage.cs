using UnityEngine;
using System.Collections.Generic;

public class ItemUsage : MonoBehaviour
{
    public void UseItem(string itemId, Player player, Enemy target = null)
    {
        ItemBase item = ItemDatabase.Instance.GetItemById(itemId);
        if (item != null)
        {
            Debug.Log($"Sử dụng vật phẩm: {item.itemName}");
            item.ExecuteAction(player, target);
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy vật phẩm với ID: {itemId}");
        }
    }

    public void UseMultipleItems(List<string> itemIds, Player player, Enemy target = null)
    {
        foreach (string id in itemIds)
        {
            UseItem(id, player, target);
        }
    }
}