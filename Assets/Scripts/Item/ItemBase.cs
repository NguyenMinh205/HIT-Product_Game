using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemBase : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public Rarity itemRarity;
    [TextArea(1, 10)]
    public string description;
    public bool isStackable;
    public int maxStackSize = 1;

    [SerializeField] private IItemAction action;

    public void ExecuteAction(GameObject player = null, GameObject target = null, float value = 0)
    {
        if (action == null)
        {
            action = ItemDatabase.Instance.CreateItemAction(id);
        }

        if (action != null)
        {
            action.Execute(player, target, value);
        }
        else
        {
            Debug.LogWarning($"Vật phẩm {itemName} không có hành động được gán cho ID: {id}");
        }
    }
}