using DG.Tweening;
using System;
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
    private ItemInventory selectedItem;
    private int upgradeFreeTurn = 0;

    private void Awake()
    {
        upgradeBtn.onClick.AddListener(UpgradeSelectedItem);
    }

    public void Init()
    {
        inventory = GamePlayController.Instance.PlayerController.TotalInventory;
        upgradeFreeTurn = GamePlayController.Instance.PlayerController.CurPlayerStat.UpgradeFreeTurn;
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
            ItemBase itemBase = item.GetItemBase();
            if (itemBase != null)
            {
                ui.Init(item, itemBase, ShowDetail, item.quantity);
            }
        }
    }

    private void DeleteInventoryList()
    {
        foreach (Transform child in listItemStore)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowDetail(ItemInventory inventoryItem)
    {
        selectedItem = inventoryItem;
        ItemBase itemBase = ItemDatabase.Instance.GetItemById(inventoryItem.itemId);
        if (inventoryItem != null && !inventoryItem.isUpgraded && itemBase?.upgradedItem != null)
        {
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
            itemDetailBeforeUpgrade.SetActive(false);
            itemDetailAfterUpgrade.SetActive(false);
            selectedItem = null;
        }
        UpdateUpgradeCost();
    }

    public void UpgradeSelectedItem()
    {
        Debug.LogError("Upgrade Selected Item");
        if (selectedItem != null)
        {
            int reduceCoin = (int)Math.Floor(coinUpgrade * GamePlayController.Instance.PlayerController.CurPlayerStat.PriceReduction);
            if (upgradeFreeTurn > 0)
            {
                Upgrade();
                upgradeFreeTurn--;
            }    
            else if (GamePlayController.Instance.PlayerController.CurPlayerStat.Coin >= (coinUpgrade - reduceCoin))
            {
                GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCoin(-(coinUpgrade - reduceCoin));
                Upgrade();
            }
            else
            {
                Debug.LogWarning("Không đủ coin để nâng cấp!");
            }
        }
        UpdateUpgradeCost();
    }

    public void Upgrade()
    {
        inventory.UpgradeItem(selectedItem);
        LoadInventoryList();
        itemDetailBeforeUpgrade.SetActive(false);
        itemDetailAfterUpgrade.SetActive(false);
        selectedItem = null;
    }    

    private void UpdateUpgradeCost()
    {
        if (selectedItem != null && selectedItem.CanUpgrade && upgradeFreeTurn <= 0)
        {
            upgradeCostText.text = coinUpgrade.ToString();
            upgradeCostText.color = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin >= coinUpgrade ? Color.white : Color.red;
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