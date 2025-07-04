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
            string id = collision.GetComponent<Item>().ID;
            CheckID(id);
            ItemController.Instance.DeleteItemOutBasket(collision.GetComponent<Item>());
            PoolingManager.Despawn(collision.gameObject);
            ItemController.Instance.CheckNextTurn();
        }
    }
    public void CheckID(string id)
    {
        switch(id)
        {
            case "1":
                Sword();
                break;

            case "3":
                break;

            case "0":
                Shield();
                break;

            default:
                break;
        }
    }

    public void Sword()
    {
        //player.Attack(GameController.Instance.enemyController.ListEnemy[0].gameObject, 10);
    }
    public void Shield()
    {
        //player.Armor += 5;
        //player.Info.UpdateArmor();
    }
}
