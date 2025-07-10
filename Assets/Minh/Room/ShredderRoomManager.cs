using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShredderRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject itemInvenPrefab;
    [SerializeField] private Transform listItemStore;
    [SerializeField] private Transform shredArea;

    [Space]
    [Header("Chi tiết item")]
    [SerializeField] private GameObject itemDetail;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;

    [SerializeField] private TextMeshProUGUI shredCostText; // Hiển thị tổng chi phí
    [SerializeField] private int shredCostPerItem = 10;
    private int numItemToShred = 0;
    private Inventory inventory;
    private List<ItemInventoryUI> itemsToShred = new List<ItemInventoryUI>();

    private void OnEnable()
    {
        inventory = GamePlayController.Instance.PlayerController.TotalInventory;
        LoadInventoryList();
        UpdateShredCost();
    }

    private void LoadInventoryList()
    {
        DeleteInventoryList();
        foreach (ItemInventory item in inventory.Items)
        {
            if (item.quantity > 0)
            {
                GameObject newItemInventoryPrefab = Instantiate(itemInvenPrefab, listItemStore);
                ItemInventoryUI ui = newItemInventoryPrefab.GetComponent<ItemInventoryUI>();
                ui.Init(item.itemBase, ShowDetail, item.quantity);
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

    private void DeleteShredList()
    {
        foreach (Transform child in shredArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowDetail(ItemBase itemBase)
    {
        detailIcon.sprite = itemBase.icon;
        detailIcon.SetNativeSize();
        detailIcon.rectTransform.sizeDelta *= 0.85f;
        detailName.text = itemBase.itemName;
        detailDescription.text = itemBase.description;

        foreach (Transform child in listItemStore)
        {
            ItemInventoryUI ui = child.GetComponent<ItemInventoryUI>();
            if (ui != null && ui.Data == itemBase && int.Parse(ui.NumOfItem.text) > 0)
            {
                int currentItemQuantity = int.Parse(ui.NumOfItem.text);
                currentItemQuantity--;
                ui.NumOfItem.text = currentItemQuantity.ToString();
                if (currentItemQuantity == 0)
                {
                    Destroy(child.gameObject);
                }

                // Tạo hoặc cập nhật item trong shred
                ItemInventoryUI shredUI = null;
                foreach (Transform shredChild in shredArea)
                {
                    ItemInventoryUI tempUI = shredChild.GetComponent<ItemInventoryUI>();
                    if (tempUI != null && tempUI.Data == itemBase)
                    {
                        shredUI = tempUI;
                        break;
                    }
                }
                if (shredUI == null)
                {
                    GameObject newShredItem = Instantiate(itemInvenPrefab, shredArea);
                    shredUI = newShredItem.GetComponent<ItemInventoryUI>();
                    shredUI.Init(itemBase, ReturnItem, 0);
                    itemsToShred.Add(shredUI);
                }
                int shredQuantity = int.Parse(shredUI.NumOfItem.text);
                shredUI.NumOfItem.text = (shredQuantity + 1).ToString();
                numItemToShred++; // Tăng số lượng item sẽ shred

                break; // Giảm 1 item mỗi lần nhấp
            }
        }
        LoadInventoryList(); // Cập nhật lại danh sách
        UpdateShredCost();
    }

    private void ReturnItem(ItemBase itemBase)
    {
        foreach (Transform child in shredArea)
        {
            ItemInventoryUI ui = child.GetComponent<ItemInventoryUI>();
            if (ui != null && ui.Data == itemBase && int.Parse(ui.NumOfItem.text) > 0)
            {
                int currentShredQuantity = int.Parse(ui.NumOfItem.text);
                currentShredQuantity--;
                ui.NumOfItem.text = currentShredQuantity.ToString();
                if (currentShredQuantity == 0)
                {
                    itemsToShred.Remove(ui);
                    Destroy(child.gameObject);
                }
                numItemToShred--;

                ItemInventoryUI storeUI = null;
                foreach (Transform storeChild in listItemStore)
                {
                    ItemInventoryUI tempUI = storeChild.GetComponent<ItemInventoryUI>();
                    if (tempUI != null && tempUI.Data == itemBase)
                    {
                        storeUI = tempUI;
                        break;
                    }
                }
                if (storeUI == null)
                {
                    GameObject newStoreItem = Instantiate(itemInvenPrefab, listItemStore);
                    storeUI = newStoreItem.GetComponent<ItemInventoryUI>();
                    storeUI.Init(itemBase, ShowDetail, 0);
                }
                int storeQuantity = int.Parse(storeUI.NumOfItem.text);
                storeUI.NumOfItem.text = (storeQuantity + 1).ToString();

                break;
            }
        }
        LoadInventoryList();
        UpdateShredCost();
    }

    private void UpdateShredCost()
    {
        int totalCost = numItemToShred * shredCostPerItem;
        shredCostText.text = totalCost.ToString();
    }

    public void ShredItems()
    {
        int totalCost = numItemToShred * shredCostPerItem;
        if (GamePlayController.Instance.PlayerController.PlayerStat.Coin >= totalCost)
        {
            GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(-totalCost);
            foreach (var itemUI in itemsToShred.ToList())
            {
                ItemInventory invItem = inventory.Items.Find(i => i.itemBase == itemUI.Data);
                if (invItem != null)
                {
                    if (int.TryParse(itemUI.NumOfItem.text, out int quantity))
                    {
                        invItem.quantity -= quantity;
                        if (invItem.quantity <= 0)
                        {
                            inventory.Items.Remove(invItem);
                        }
                    }
                }
                itemsToShred.Remove(itemUI);
                Destroy(itemUI.gameObject);
            }
            numItemToShred = 0;
            DeleteInventoryList();
            DeleteShredList();
            LoadInventoryList();
            UpdateShredCost();
        }
    }

    private void OnDisable()
    {
        DeleteInventoryList();
    }
}