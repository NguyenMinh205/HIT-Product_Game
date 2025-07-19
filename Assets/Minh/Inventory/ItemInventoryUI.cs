using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private ItemBase data;
    [SerializeField] private TextMeshProUGUI numOfItem;
    private ItemInventory inventoryItem;
    public ItemBase Data => data;
    public TextMeshProUGUI NumOfItem => numOfItem;

    public void Init(ItemInventory inventoryItem, ItemBase itemBase, Action<ItemInventory> action, int quantity = 1)
    {
        this.inventoryItem = inventoryItem;
        this.data = itemBase;
        iconImage.sprite = inventoryItem.Icon;
        iconImage.SetNativeSize();
        iconImage.rectTransform.sizeDelta *= 0.75f;
        numOfItem.text = quantity.ToString();

        // Tạm thời tắt logic kiểm tra nâng cấp để tránh xung đột, có thể bật lại sau
        // if (inventoryItem.isUpgraded || data.upgradedItem == null)
        // {
        //     Color color = iconImage.color;
        //     color.a = 200f / 255f;
        //     iconImage.color = color;
        //     GetComponent<Button>().interactable = false;
        // }
        // else
        // {
        //     Color color = iconImage.color;
        //     color.a = 1f;
        //     iconImage.color = color;
        //     GetComponent<Button>().interactable = true;
        // }

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => action(inventoryItem));
    }
}