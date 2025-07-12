using UnityEngine;

[System.Serializable]
public class ItemInventory
{
    public ItemBase itemBase;
    public int quantity;

    public ItemInventory(ItemBase itemBase, int qty)
    {
        this.itemBase = itemBase;
        quantity = qty;
    }

    public string ItemId => itemBase != null ? itemBase.id : string.Empty;
    public Sprite Icon => itemBase != null ? itemBase.icon : null;

    public bool CanUpgrade => itemBase != null && !itemBase.isUpgraded && itemBase.upgradedItem != null;
}