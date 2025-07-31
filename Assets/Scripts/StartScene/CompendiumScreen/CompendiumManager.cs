using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEditor.Progress;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using DG.Tweening;

public enum CompendiumType
{
    Item,
    Perk
}


public class CompendiumManager : Singleton<CompendiumManager>
{
    [Header("Prefab Setting")]
    [SerializeField] private Transform itemList;
    [SerializeField] private Transform perkList;
    [SerializeField] private ScrollRect scroll;

    [Space]
    [Header("Show Detail Setting")]
    [SerializeField] private GameObject detailObj;
    [SerializeField] private Sprite itemBG;
    [SerializeField] private Sprite perkBG;
    [SerializeField] private Image detailBG;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;

    [Space]
    [Header("Control")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI currentTypeTxt;

    [SerializeField] private List<ItemBase> items;
    [SerializeField] private List<PerkBase> perks;
    private List<CompendiumType> compendiumTypes;
    private int currentIndex = 0;

    void Start()
    {
        compendiumTypes = new List<CompendiumType>((CompendiumType[])System.Enum.GetValues(typeof(CompendiumType)));
        items = Resources.LoadAll<ItemBase>("ItemSO").ToList();
        perks = Resources.LoadAll<PerkBase>("PerkSO").ToList();

        leftButton.onClick.AddListener(PreviousType);
        rightButton.onClick.AddListener(NextType);

        LoadCompendium();
    }

    void LoadCompendium()
    {
        CompendiumType currentType = compendiumTypes[currentIndex];
        currentTypeTxt.text = currentType.ToString();
        detailIcon.gameObject.SetActive(false);
        detailDescription.SetText("");
        detailName.SetText("");

        if (currentType == CompendiumType.Item)
        {
            detailBG.sprite = itemBG;
            detailBG.color = Color.white;
            scroll.content = itemList.GetComponent<RectTransform>();
            itemList.gameObject.SetActive(true);
            perkList.gameObject.SetActive(false);
        }
        else if (currentType == CompendiumType.Perk)
        {
            detailBG.sprite = perkBG;
            detailBG.color = Color.white;
            scroll.content = perkList.GetComponent<RectTransform>();
            itemList.gameObject.SetActive(false);
            perkList.gameObject.SetActive(true);
        }
    }

    public void ShowDetail(ScriptableObject data)
    {
        detailIcon.gameObject.SetActive(true);
        CanvasGroup canvasGroup = detailObj.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.25f;
        canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad).OnComplete(() => Canvas.ForceUpdateCanvases());

        if (data is ItemBase item)
        {
            detailIcon.sprite = item.icon;
            detailIcon.SetNativeSize();
            detailIcon.rectTransform.sizeDelta *= 0.7f;
            detailName.text = item.itemName;
            detailDescription.text = item.description;
            detailBG.color = item.itemRarity switch
            {
                Rarity.Common => commonColor,
                Rarity.Rare => rareColor,
                Rarity.Epic => epicColor,
                _ => Color.white
            };
        }
        else if (data is PerkBase perk)
        {
            detailIcon.sprite = perk.icon;
            detailIcon.SetNativeSize();
            detailIcon.rectTransform.sizeDelta *= 0.75f;
            detailName.text = perk.perkName;
            detailDescription.text = perk.description;
        }
    }

    public void PreviousType()
    {
        currentIndex = (currentIndex - 1 + compendiumTypes.Count) % compendiumTypes.Count;
        LoadCompendium();
    }

    public void NextType()
    {
        currentIndex = (currentIndex + 1) % compendiumTypes.Count;
        LoadCompendium();
    }
}
