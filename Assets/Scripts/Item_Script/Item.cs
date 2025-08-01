using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string idItem;
    [SerializeField] private string nameItem;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] public Rarity itemRarity;
    [SerializeField] public string description;
    [SerializeField] public bool isStackable;
    [SerializeField] public int maxStackSize = 99;

    [SerializeField] private float moveForce;

    [SerializeField] private PolygonCollider2D poly;
    [SerializeField] private Rigidbody2D rb;

    private ItemBase _itemBase;

    private bool isPickUp = false;
    private bool isMove;

    public ItemBase ItemBase
    {
        get => _itemBase;
    }
    public string ID
    {
        get => idItem;
    }
    public SpriteRenderer SR
    {
        get => this.sr;
        set => this.sr = value;
    }
    public Sprite Sprite => sr.sprite;

    private void OnEnable()
    {
        isMove = false;
    }
    public void Init(ItemBase itemBase)
    {
        idItem = itemBase.id;
        nameItem = itemBase.itemName;
        sr.sprite = itemBase.icon;
        ResetCollider2D();
        itemRarity = itemBase.itemRarity;
        description = itemBase.description;
        isStackable = itemBase.isStackable;
        maxStackSize = itemBase.maxStackSize;

        _itemBase = itemBase;

        if (idItem == "Buf01" || idItem == "Buf23")
        {
            _itemBase.Action = ItemActionFactory.CreateItemAction(idItem);
            _itemBase.Action.Execute(null, null);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Basket"))
        {
            ObserverManager<IDItem>.PostEven(IDItem.ItemChange, this);
        }
    }

    private void ResetCollider2D()
    {
        if (poly == null || sr.sprite == null) return;

        poly.pathCount = sr.sprite.GetPhysicsShapeCount();

        // new paths variable
        List<Vector2> path = new List<Vector2>();


        // loop path count
        for (int i = 0; i < poly.pathCount; i++)
        {
            // clear
            path.Clear();
            // get shape
            sr.sprite.GetPhysicsShape(i, path);
            // set path
            poly.SetPath(i, path.ToArray());
        }
    }

}
