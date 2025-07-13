using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Space]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private int coinUpgrade = 10;

    private Inventory inventory;
    private ItemBase selectedItem;

    private void Awake()
    {
        upgradeBtn.onClick.AddListener(UpgradeSelectedItem);
    }

    public void Init()
    {
        inventory = GamePlayController.Instance.PlayerController.TotalInventory;
        LoadInventoryList();
        UpdateUpgradeCost();
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
        // Tìm ItemInventory tương ứng để kiểm tra CanUpgrade
        ItemInventory inventoryItem = inventory.Items.Find(item => item.itemBase == itemBase);
        if (inventoryItem != null && !itemBase.isUpgraded && itemBase.upgradedItem != null)
        {
            selectedItem = itemBase;

            CanvasGroup canvasGroupBefore = itemDetailBeforeUpgrade.GetComponent<CanvasGroup>();
            if (canvasGroupBefore == null)
            {
                canvasGroupBefore = itemDetailBeforeUpgrade.AddComponent<CanvasGroup>();
            }

            canvasGroupBefore.alpha = 0f;

            itemDetailBeforeUpgrade.SetActive(true);
            detailIconBefore.sprite = itemBase.icon;
            detailIconBefore.SetNativeSize();
            detailIconBefore.rectTransform.sizeDelta *= 0.85f;
            detailNameBefore.text = itemBase.itemName;
            detailDescriptionBefore.text = itemBase.description;

            canvasGroupBefore.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);

            // Hiển thị chi tiết sau nâng cấp

            CanvasGroup canvasGroupAfter = itemDetailAfterUpgrade.GetComponent<CanvasGroup>();
            if (canvasGroupAfter == null)
            {
                canvasGroupAfter = itemDetailAfterUpgrade.AddComponent<CanvasGroup>();
            }

            canvasGroupAfter.alpha = 0f;

            itemDetailAfterUpgrade.SetActive(true);
            detailIconAfter.sprite = itemBase.upgradedItem.icon;
            detailIconAfter.SetNativeSize();
            detailIconAfter.rectTransform.sizeDelta *= 0.85f;
            detailNameAfter.text = itemBase.upgradedItem.itemName;
            detailDescriptionAfter.text = itemBase.upgradedItem.description;

            canvasGroupAfter.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
        }
        else
        {
            // Ẩn thông tin nếu vật phẩm không thể nâng cấp
            itemDetailBeforeUpgrade.SetActive(false);
            itemDetailAfterUpgrade.SetActive(false);
            selectedItem = null;
        }
        UpdateUpgradeCost();
    }

    public void UpgradeSelectedItem()
    {
        if (selectedItem != null)
        {
            ItemInventory inventoryItem = inventory.Items.Find(item => item.itemBase == selectedItem);
            if (inventoryItem != null && !inventoryItem.itemBase.isUpgraded && inventoryItem.itemBase.upgradedItem != null)
            {
                if (GamePlayController.Instance.PlayerController.PlayerStat.Coin >= coinUpgrade)
                {
                    GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(-coinUpgrade);
                    inventory.UpgradeItem(inventoryItem);
                    LoadInventoryList();
                    // Ẩn thông tin sau khi nâng cấp
                    itemDetailBeforeUpgrade.SetActive(false);
                    itemDetailAfterUpgrade.SetActive(false);
                    selectedItem = null;
                }
                else
                {
                    Debug.LogWarning("Không đủ coin để nâng cấp!");
                }
            }
        }
        UpdateUpgradeCost();
    }

    private void UpdateUpgradeCost()
    {
        if (selectedItem != null && !selectedItem.isUpgraded && selectedItem.upgradedItem != null)
        {
            upgradeCostText.text = coinUpgrade.ToString();
        }
        else
        {
            upgradeCostText.text = "0";
        }
    }

    private void OnDisable()
    {
        DeleteInventoryList();
    }
}