using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] private List<ItemBase> items;

    private void Start()
    {
        items = Resources.LoadAll<ItemBase>("ItemSO").ToList();
    }

    public ItemBase GetItemById(string id)
    {
        return items.Find(item => item.id == id);
    }

    public ItemBase GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }

    public IItemAction CreateItemAction(string id)
    {
        return ItemActionFactory.CreateItemAction(id);
    }
}