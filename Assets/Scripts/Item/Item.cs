using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string idItem;
    [SerializeField] private string nameItem;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Rarity itemRarity;
    [SerializeField] public string description;
    [SerializeField] public bool isStackable;
    [SerializeField] public int maxStackSize = 99;

    [SerializeField] private GameObject balloon;
    [SerializeField] private float moveForce;

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
        get => this.spriteRenderer;
        set => this.spriteRenderer = value;
    }
    private void OnEnable()
    {
        //isPickUp = false;
        isMove = false;
        //balloon.SetActive(false);
    }
    public void Init(ItemBase itemBase)
    {
        idItem = itemBase.id;
        nameItem = itemBase.itemName;
        spriteRenderer.sprite = itemBase.icon;
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
        if(collision.CompareTag("Basket") && !isPickUp)
        {
            Debug.Log("Item in Basket");
            isPickUp = true;

            this.GetComponent<PolygonCollider2D>().isTrigger = true;
            this.GetComponent<Rigidbody2D>().simulated = false;
            ObserverManager<ItemMove>.PostEven(ItemMove.AddItemToMove, this);

            ObserverManager<IDItem>.PostEven(IDItem.ItemChange, this);
        }
    }

    private void ResetCollider2D()
    {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (poly == null || sr.sprite == null) return;

        poly.pathCount = 0;

        int shapeCount = sr.sprite.GetPhysicsShapeCount();
        poly.pathCount = shapeCount;

        for (int i = 0; i < shapeCount; i++)
        {
            var shape = new List<Vector2>();
            sr.sprite.GetPhysicsShape(i, shape);
            poly.SetPath(i, shape);
        }
    }
    public void SetBalloon(bool val)
    {
        balloon.SetActive(val);
    }
}
