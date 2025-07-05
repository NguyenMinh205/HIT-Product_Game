using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string idItem;
    [SerializeField] private string nameItem;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Rarity itemRarity;
    [SerializeField] public string description;
    [SerializeField] public bool isStackable;
    [SerializeField] public int maxStackSize = 1;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject balloon;
    [SerializeField] private float moveForce;


    private bool isPickUp;
    private bool isMove;

    public string ID
    {
        get => idItem;
    }
    public bool IsPickUp
    {
        get => this.isPickUp;
        set => this.isPickUp = value;
    }
    private void OnEnable()
    {
        isPickUp = false;
        isMove = false;
        balloon.SetActive(false);
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
    }
    private void Update()
    {
        if(isPickUp)
        {
            MoveItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Basket"))
        {
            Debug.Log("Item in Basket");
            isPickUp = true;
            balloon.SetActive(true);
            //ItemController.Instance.ChangeBoxToBasket(gameObject.GetComponent<Item>());
        }
        else if(collision.CompareTag("Check"))
        {
            isMove = true;
        }
    }

    public void MoveItem()
    {
        Debug.Log("Move Item");
        if (isMove)
        {
            //rb.velocity = (GameController.Instance.playerController.PosPlayer - transform.position) * moveForce;
        }
        else
            rb.velocity = Vector2.down * moveForce;
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
}
