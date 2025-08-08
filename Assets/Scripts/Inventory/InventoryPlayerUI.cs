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
    [SerializeField] private Transform invenPerkStore;

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

    [Header("Title Setup")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private List<string> listTitle;

    private int indexTitle = 0;
    private bool isTransitioning = false;
    private Inventory inventory;
    private ItemInventory selectedItem;

    private void OnEnable()
    {
        indexTitle = 0;
    }

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
        indexTitle = 0;
        SetTitle();
        SetList();
        LoadInventoryList();
    }

    private void LoadInventoryList()
    {
        DeleteInventoryList();
        foreach (ItemInventory item in inventory.Items)
        {
            GameObject newItemInventoryPrefab = Instantiate(itemInvenPrefab, invenItemStore);
            ItemInventoryUI ui = newItemInventoryPrefab.GetComponent<ItemInventoryUI>();
            ItemBase itemBase = item.GetItemBase();
            if (itemBase != null)
            {
                ui.Init(item, itemBase, ShowDetail, item.quantity);
            }
        }
        foreach (UiPerk perk in UiPerksList.Instance.Perks)
        {
            GameObject newItemInventoryPrefab = Instantiate(itemInvenPrefab, invenPerkStore);
            ItemInventoryUI ui = newItemInventoryPrefab.GetComponent<ItemInventoryUI>();
            ui.Init(perk, ShowDetail);
        }
        invenPerkStore.gameObject.SetActive(false);
    }

    private void DeleteInventoryList()
    {
        foreach (Transform child in invenItemStore)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in invenPerkStore)
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

    public void ShowDetail(UiPerk uiPerk)
    {
        if (uiPerk == null)
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

        detailIcon.sprite = uiPerk.Icon.sprite;
        detailIcon.SetNativeSize();
        detailIcon.rectTransform.sizeDelta *= 0.85f;
        detailName.text = uiPerk.perkName;
        detailDescription.text = uiPerk.description;
        itemDetailBG.color = Color.white;

        canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
    }

    public void OnLeft()
    {
        indexTitle--;
        if (indexTitle < 0)
        {
            indexTitle = listTitle.Count - 1;
        }
        AnimateTitleChange(indexTitle, true);
    }
    public void OnRight()
    {
        indexTitle++;
        if (indexTitle > listTitle.Count - 1)
        {
            indexTitle = 0;
        }
        AnimateTitleChange(indexTitle, false);
    }
    public void SetTitle()
    {
        title.text = listTitle[indexTitle];
    }
    public void SetList()
    {
        if (title.text == "Items")
        {
            invenItemStore.gameObject.SetActive(true);
            invenPerkStore.gameObject.SetActive(false);
        }
        else if (title.text == "Perks")
        {
            invenItemStore.gameObject.SetActive(false);
            invenPerkStore.gameObject.SetActive(true);
        }

    }

    private void AnimateTitleChange(int newIndex, bool slideLeft)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        float slideDistance = 50f;
        float duration = 0.3f;

        Vector2 startPos = contentPanel.anchoredPosition;
        Vector2 offscreenPos = startPos + new Vector2(slideLeft ? slideDistance : -slideDistance, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(contentGroup.DOFade(0f, duration / 2));
        seq.Join(contentPanel.DOAnchorPos(offscreenPos, duration / 2));

        seq.AppendCallback(() =>
        {
            indexTitle = newIndex;
            SetTitle();
            SetList();
            contentPanel.anchoredPosition = startPos - new Vector2(slideLeft ? slideDistance : -slideDistance, 0);
        });

        seq.Append(contentPanel.DOAnchorPos(startPos, duration / 2));
        seq.Join(contentGroup.DOFade(1f, duration / 2));

        seq.OnComplete(() => isTransitioning = false);
    }
}
