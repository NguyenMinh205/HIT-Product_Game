using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemPrefabs : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject balloon;
    [SerializeField] private float moveForce;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private string idItem;
    private string nameItem;

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
    public void Init(string id, string name, Sprite icon)
    {
        idItem = id;
        nameItem = name;
        spriteRenderer.sprite = icon;
        ResetCollider2D();
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
            ItemController.Instance.ChangeBoxToBasket(gameObject.GetComponent<ItemPrefabs>());
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
