using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            Debug.Log("Item trigger with player");

            ObserverManager<IDItem>.PostEven(IDItem.ItemPlayer, collision.GetComponent<Item>());
            //ItemController.Instance.DeleteItemOutBasket(collision.GetComponent<Item>());
            PoolingManager.Despawn(collision.gameObject);
            //ItemController.Instance.CheckNextTurn();

        }
    }

}
