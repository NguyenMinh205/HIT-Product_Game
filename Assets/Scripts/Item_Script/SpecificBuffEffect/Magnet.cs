using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, IItemAction
{
    private float attractionRadius = 5f;
    private float attractionForce = 10f;
    private bool isActive = false;
    private CircleCollider2D attractionCollider;
    private List<Item> itemsInRange = new List<Item>();
    private ItemController itemController;

    public void Buff()
    {
        if (isActive) return;

        var gamePlayController = GamePlayController.Instance;
        if (gamePlayController == null || gamePlayController.ItemController == null)
        {
            return;
        }

        itemController = gamePlayController.ItemController;

        isActive = true;

        attractionCollider = gameObject.GetComponent<CircleCollider2D>();
        if (attractionCollider == null)
        {
            attractionCollider = gameObject.AddComponent<CircleCollider2D>();
            attractionCollider.radius = attractionRadius;
            attractionCollider.isTrigger = true;
        }
    }

    public void Execute(Player player, Enemy target)
    {
        if (this == null)
        {
            Debug.LogError("Magnet script is null.");
            return;
        }
        else if (gameObject == null)
        {
            Debug.LogError(" game object is null.");
            return;
        }    

            Buff();
    }

    public void Upgrade()
    {

    }

    private void Update()
    {
        if (isActive)
        {
            AttractMetalItems();
        }
    }

    private void AttractMetalItems()
    {
        foreach (Item item in itemsInRange)
        {
            if (item != null && item.ItemBase != null && item.ItemBase.isMetal)
            {
                Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
                if (itemRb != null && itemRb.simulated)
                {
                    Vector3 magnetPos = this.transform.position;
                    Vector3 itemPos = item.transform.position;
                    Vector3 direction = (magnetPos - itemPos).normalized;
                    itemRb.AddForce(direction * attractionForce * Time.deltaTime, ForceMode2D.Force);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && item.ItemBase != null && item.ItemBase.isMetal && !itemsInRange.Contains(item) && itemController.ListItemInBox.Contains(item))
        {
            itemsInRange.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && itemsInRange.Contains(item))
        {
            itemsInRange.Remove(item);
        }
    }

    public void Deactivate()
    {
        isActive = false;
        itemsInRange.Clear();
    }

    private void OnDestroy()
    {
        Deactivate();
    }
}