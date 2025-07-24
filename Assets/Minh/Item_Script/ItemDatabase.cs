using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] private List<ItemBase> items;
    [SerializeField] private List<ItemBase> itemsByEnemy;
    [SerializeField] private List<ItemBase> commonItems = new List<ItemBase>();
    [SerializeField] private List<ItemBase> rareItems = new List<ItemBase>();
    [SerializeField] private List<ItemBase> epicItems = new List<ItemBase>();

    private void Start()
    {
        //items = Resources.LoadAll<ItemBase>("ItemSO").ToList();
        foreach (var item in items)
        {
            switch (item.itemRarity)
            {
                case Rarity.Common:
                    commonItems.Add(item);
                    break;
                case Rarity.Rare:
                    rareItems.Add(item);
                    break;
                case Rarity.Epic:
                    epicItems.Add(item);
                    break;
            }
        }
    }

    public ItemBase GetItemById(string id)
    {
        return items.Find(item => item.id == id);
    }
    public ItemBase GetItemEnemyById(string id)
    {
        return itemsByEnemy.Find(item => item.id == id);
    }

    public ItemBase GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }

    public ItemBase GetRandomItem(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return commonItems.Count > 0 ? commonItems[Random.Range(0, commonItems.Count)] : null;
            case Rarity.Rare:
                return rareItems.Count > 0 ? rareItems[Random.Range(0, rareItems.Count)] : null;
            case Rarity.Epic:
                return epicItems.Count > 0 ? epicItems[Random.Range(0, epicItems.Count)] : null;
            default:
                return null;
        }
    }

    public List<ItemBase> GetItems()
    {
        return items;
    }

    public IItemAction CreateItemAction(string id)
    {
        return ItemActionFactory.CreateItemAction(id);
    }
}