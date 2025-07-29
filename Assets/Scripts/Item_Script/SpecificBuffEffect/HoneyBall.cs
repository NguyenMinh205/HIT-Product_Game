using UnityEngine;

public class HoneyBall : MonoBehaviour, IItemAction
{
    private bool isActive = false;
    private ItemController itemController;

    public void Buff()
    {
        if (isActive) return;

        itemController = GamePlayController.Instance.ItemController;
        if (itemController == null)
        {
            Debug.LogWarning("Không tìm thấy ItemController!");
            return;
        }

        isActive = true;
    }

    public void Execute(Player player, Enemy target)
    {
        Buff();
    }

    public void Upgrade()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null && item != this.GetComponent<Item>() && itemController.ListItemInBox.Contains(item))
        {
            item.transform.SetParent(transform);
            Debug.Log($"Vật phẩm {item.ID} dính vào HoneyBall!");
        }
    }

    public void Deactivate()
    {
        isActive = false;
    }

    private void OnDestroy()
    {
        Deactivate();
    }
}