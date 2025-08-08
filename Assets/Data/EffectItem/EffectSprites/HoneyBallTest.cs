using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBallTest : MonoBehaviour
{
    [SerializeField] private Item itemThis;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            if (item == itemThis)
            {
                return;
            }
            item.transform.SetParent(transform);
            Debug.Log($"Vật phẩm {item.ID} dính vào HoneyBall!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();

        if(item != null)
        {
            if (item == itemThis)
            {
                return;
            }
            item.transform.SetParent(transform.parent);
        }
    }
}
