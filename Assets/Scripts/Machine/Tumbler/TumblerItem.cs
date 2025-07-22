using UnityEngine;
using UnityEngine.UI;

public class TumblerItem : MonoBehaviour
{
    private PerkBase perkBase;
    [SerializeField] private Image icon;
    private Rigidbody2D _rb;
    public PerkBase PerkBase => perkBase;

    public void Init(PerkBase perk)
    {
        perkBase = perk;
        icon.sprite = perk.icon;
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