using UnityEngine;
using UnityEngine.UI;

public class PachinkoItem : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Image itemImage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Sprite itemSprite = null)
    {
        if (itemImage != null && itemSprite != null)
        {
            itemImage.sprite = itemSprite;
            itemImage.enabled = true;
        }
    }

    public void SetDrop()
    {
        _rb.isKinematic = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == PachinkoMachine.Instance.Box.BasketTrigger)
            PachinkoMachine.Instance.EndGame(true);
        else if (collision == PachinkoMachine.Instance.Box.FloorTrigger)
            PachinkoMachine.Instance.EndGame(false);
    }
}