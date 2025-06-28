using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase instance;
    public static ItemDatabase Instance => instance;

    [SerializeField] private List<ItemBase> items;
    [SerializeField] private List<PerkBase> perks;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        items = Resources.LoadAll<ItemBase>("ItemSO").ToList();
        perks = Resources.LoadAll<PerkBase>("PerkSO").ToList();
    }

    public ItemBase GetItemById(string id)
    {
        return items.Find(item => item.id == id);
    }

    public PerkBase GetPerkById(string id)
    {
        return perks.Find(perk => perk.id == id);
    }

    public IItemAction CreateItemAction(string id)
    {
        return ItemActionFactory.CreateItemAction(id);
    }
}