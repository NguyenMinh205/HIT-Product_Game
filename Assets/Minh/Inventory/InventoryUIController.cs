using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject itemPrefab;

    public void UpdateInventoryDisplay(List<ItemInventory> items)
    {
        // Xóa các UI hiện tại
        foreach (Transform child in inventoryParent)
        {
            Destroy(child.gameObject);
        }

        // Hiển thị các item
        foreach (ItemInventory item in items)
        {
            if (item.quantity > 0 && item.itemBase != null)
            {
                GameObject itemUI = Instantiate(itemPrefab, inventoryParent);
                Image iconImage = itemUI.GetComponentInChildren<Image>();
                Text quantityText = itemUI.GetComponentInChildren<Text>();

                if (iconImage != null)
                {
                    iconImage.sprite = item.Icon;
                }
                if (quantityText != null)
                {
                    quantityText.text = item.quantity.ToString();
                }
            }
        }
    }
}