using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject dime;
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private GameObject itemInvenPrefab;
    [SerializeField] private Transform invenItemStore;

    [Space]
    [Header("Item Detail")]
    [SerializeField] private GameObject itemDetail;
    [SerializeField] private Image itemDetailBG;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;


    private Inventory inventory;
    private ItemInventory selectedItem;

    public void PauseAndShowInventory()
    {
        AudioManager.Instance.PlaySoundClickButton();
        dime.SetActive(true);
        inventoryView.SetActive(true);
        Init();
    }

    public void BackToGame()
    {
        AudioManager.Instance.PlaySoundClickButton();
        dime.SetActive(false);
        inventoryView.SetActive(false);
    }

    public void Init()
    {
        inventory = GamePlayController.Instance.PlayerController.TotalInventory;
        LoadInventoryList();
    }

    private void LoadInventoryList()
    {
        DeleteInventoryList();
        foreach (ItemInventory item in inventory.Items)
        {
            //GameObject newItemInventoryPrefab = PoolingManager.Spawn(itemInvenPrefab, this.transform.position, Quaternion.identity, invenItemStore);
            GameObject newItemInventoryPrefab = Instantiate(itemInvenPrefab, invenItemStore);
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
        foreach (Transform child in invenItemStore)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowDetail(ItemInventory inventoryItem)
    {
        ItemBase itemBase = inventoryItem.GetItemBase();
        if (itemBase == null)
        {
            itemDetail.SetActive(false);
            return;
        }

        CanvasGroup canvasGroup = itemDetail.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = itemDetail.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        itemDetail.SetActive(true);

        detailIcon.sprite = itemBase.icon;
        detailIcon.SetNativeSize();
        detailIcon.rectTransform.sizeDelta *= 0.85f;
        detailName.text = itemBase.itemName;
        detailDescription.text = itemBase.description;
        itemDetailBG.color = itemBase.itemRarity switch
        {
            Rarity.Common => commonColor,
            Rarity.Rare => rareColor,
            Rarity.Epic => epicColor,
            _ => Color.white
        };

        canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
    }
}
