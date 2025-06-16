using UnityEngine;

public class TumblerItem : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == TumblerMachine.Instance.TumblerBox.ExitTrigger)
        {
            TumblerMachine.Instance.OnItemCollected(this);
        }
    }
}