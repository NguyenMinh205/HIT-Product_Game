using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private ItemBase data;
    [SerializeField] private TextMeshProUGUI numOfItem;
    [SerializeField] private Image starUpgrade;
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

        Button button = iconImage.GetComponent<Button>();

        if (GameManager.Instance.CurrentRoom == GameManager.Instance.SmithRoom)
        {
            if (inventoryItem.isUpgraded || data.upgradedItem == null)
            {
                Color color = iconImage.color;
                color.a = 200f / 255f;
                iconImage.color = color;
                button.interactable = false;
            }
            else
            {
                Color color = iconImage.color;
                color.a = 1f;
                iconImage.color = color;
                button.interactable = true;
            }
        }

        if (!itemBase.upgradedItem)
        {
            starUpgrade.gameObject.SetActive(true);
        }
        else
        {
            starUpgrade.gameObject.SetActive(false);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action(inventoryItem));
    }
}