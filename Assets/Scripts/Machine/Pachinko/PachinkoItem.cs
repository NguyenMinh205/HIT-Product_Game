using UnityEngine;
using UnityEngine.UI;

public class PachinkoItem : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Image itemImage;
    [SerializeField] private ItemBase itemBase;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(ItemBase item)
    {
        if (itemImage != null && item != null)
        {
            itemBase = item;
            itemImage.sprite = item.icon;
            itemImage.SetNativeSize();
            itemImage.rectTransform.sizeDelta *= 0.0035f;
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