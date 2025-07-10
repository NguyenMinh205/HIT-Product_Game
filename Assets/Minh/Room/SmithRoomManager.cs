using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SmithRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject itemInvenPrefab;
    [SerializeField] private Transform listItemStore;

    [Space]
    [Header("Detail Before Upgrade")]
    [SerializeField] private GameObject itemDetailBeforeUpgrade;
    [SerializeField] private Image detailIconBefore;
    [SerializeField] private TextMeshProUGUI detailNameBefore;
    [SerializeField] private TextMeshProUGUI detailDescriptionBefore;

    [Space]
    [Header("Detail After Upgrade")]
    [SerializeField] private GameObject itemDetailAfterUpgrade;
    [SerializeField] private Image detailIconAfter;
    [SerializeField] private TextMeshProUGUI detailNameAfter;
    [SerializeField] private TextMeshProUGUI detailDescriptionAfter;

    private Inventory inventory;
    private void OnEnable()
    {
        inventory = GamePlayController.Instance.PlayerController.TotalInventory;
        LoadInventoryList();
    }

    private void LoadInventoryList()
    {
        DeleteInventoryList();
        foreach (ItemInventory item in inventory.Items)
        {
            GameObject newItemInventoryPrefab = Instantiate(itemInvenPrefab, listItemStore);
            ItemInventoryUI ui = newItemInventoryPrefab.GetComponent<ItemInventoryUI>();
            ui.Init(item.itemBase, ShowDetail, item.quantity);
        }
    }

    private void DeleteInventoryList()
    {
        foreach (Transform child in listItemStore)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowDetail(ItemBase itemBase)
    {
        detailIconBefore.sprite = itemBase.icon;
        detailIconBefore.SetNativeSize();
        detailIconBefore.rectTransform.sizeDelta *= 0.85f;
        detailNameBefore.text = itemBase.itemName;
        detailDescriptionBefore.text = itemBase.description;
    }
}
