using UnityEngine;

[System.Serializable]
public class ItemInventory
{
    public string itemId;
    public int quantity;
    public bool isUpgraded;

    public ItemInventory(string itemId, int qty, bool upgraded = false)
    {
        this.itemId = itemId;
        quantity = qty;
        isUpgraded = upgraded;
    }

    public string ItemId => itemId;
    public Sprite Icon
    {
        get
        {
            ItemBase itemBase = ItemDatabase.Instance.GetItemById(itemId);
            if (itemBase != null && isUpgraded && itemBase.upgradedItem != null)
            {
                return itemBase.upgradedItem.icon;
            }
            return itemBase?.icon;
        }
    }
    public bool CanUpgrade
    {
        get
        {
            ItemBase itemBase = ItemDatabase.Instance.GetItemById(itemId);
            return itemBase != null && !isUpgraded && itemBase.upgradedItem != null;
        }
    }

    public ItemBase GetItemBase()
    {
        ItemBase itemBase = ItemDatabase.Instance.GetItemById(itemId);
        if(itemBase == null)
        {
            itemBase = ItemDatabase.Instance.GetItemEnemyById(itemId);
        }
        if (itemBase != null && isUpgraded && itemBase.upgradedItem != null)
        {
            return itemBase.upgradedItem;
        }
        return itemBase;
    }
}