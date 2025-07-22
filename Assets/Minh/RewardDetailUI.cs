using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardDetailUI : MonoBehaviour
{
    [SerializeField] private Image markChoice;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Color colorDefault;
    private ItemBase item;
    private PerkBase perk;

    public Image MarkChoice => markChoice;
    public ItemBase Item => item;
    public PerkBase Perk => perk;
    public Color ColorDefault => colorDefault;

    public void Init(ItemBase item)
    {
        this.item = item;
        itemIcon.sprite = item.icon;
        itemIcon.SetNativeSize();
        itemIcon.rectTransform.sizeDelta *= 0.7f;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        markChoice.color = colorDefault;
        GetComponent<Button>().onClick.AddListener(() => RewardManager.Instance.SelectReward(this));
    }

    public void Init(PerkBase perk)
    {
        this.perk = perk;
        itemIcon.sprite = perk.icon;
        itemIcon.SetNativeSize();
        itemIcon.rectTransform.sizeDelta *= 0.7f;
        itemName.text = perk.name;
        itemDescription.text = perk.description;
        markChoice.color = colorDefault;
        GetComponent<Button>().onClick.AddListener(() => RewardManager.Instance.SelectReward(this));
    }
}