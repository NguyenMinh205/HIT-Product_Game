using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemPrefabs : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject balloon;
    [SerializeField] private float moveForce;

    private string idItem;
    private string nameItem;

    private bool isPickUp;
    private bool isMove;
    public bool IsPickUp
    {
        get => this.isPickUp;
        set => this.isPickUp = value;
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
        }
        else if(collision.CompareTag("Check"))
        {
            isMove = true;
        }
        else if(collision.CompareTag("Player"))
        {
            Debug.Log("De Spawn");
            PoolingManager.Despawn(gameObject);
        }
    }

    public void MoveItem()
    {
        Debug.Log("Move Item");
        if (isMove)
        {
            rb.velocity = (GameController.Instance.playerController.PosPlayer - transform.position) * moveForce;
        }
        else
            rb.velocity = Vector2.down * moveForce;
    }
}
