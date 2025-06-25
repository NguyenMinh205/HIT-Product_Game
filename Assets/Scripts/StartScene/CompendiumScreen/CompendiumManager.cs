using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEditor.Progress;
using TMPro;
using System.Linq;

public enum CompendiumType
{
    Item,
    Perk
}


public class CompendiumManager : Singleton<CompendiumManager>
{
    [Header("Prefab Setting")]
    [SerializeField] private CompendiumItemUI compendiumPrefab;
    [SerializeField] private Transform contentParent;

    [Space]
    [Header("Show Detail Setting")]
    [SerializeField] private Sprite itemBG;
    [SerializeField] private Sprite perkBG;
    [SerializeField] private Image detailBG;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;

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
        foreach (Transform child in contentParent)
        {
            PoolingManager.Despawn(child.gameObject);
        }

        CompendiumType currentType = compendiumTypes[currentIndex];
        currentTypeTxt.text = currentType.ToString();


        if (currentType == CompendiumType.Item)
        {
            detailBG.sprite = itemBG;
            detailIcon.gameObject.SetActive(false);

            foreach (ItemBase item in items)
            {
                CompendiumItemUI newItem = PoolingManager.Spawn(compendiumPrefab, this.transform.position, Quaternion.identity, contentParent);
                newItem.Setup(item);
            }
        }
        else if (currentType == CompendiumType.Perk)
        {
            detailBG.sprite = perkBG;
            detailIcon.gameObject.SetActive(false);

            foreach (PerkBase perk in perks)
            {
                CompendiumItemUI newPerk = PoolingManager.Spawn(compendiumPrefab, this.transform.position, Quaternion.identity, contentParent);
                newPerk.Setup(perk);
            }
        }
    }

    public void ShowDetail(ScriptableObject data)
    {
        detailIcon.gameObject.SetActive(true);
        if (data is ItemBase item)
        {
            detailIcon.sprite = item.icon;
            detailName.text = item.itemName;
            detailDescription.text = item.description;
        }
        else if (data is PerkBase perk)
        {
            detailIcon.sprite = perk.icon;
            detailName.text = perk.perkName;
            detailDescription.text = perk.description;
        }
    }

    public void PreviousType()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = compendiumTypes.Count - 1;
        LoadCompendium();
    }

    public void NextType()
    {
        currentIndex++;
        if (currentIndex >= compendiumTypes.Count) currentIndex = 0;
        LoadCompendium();
    }
}
