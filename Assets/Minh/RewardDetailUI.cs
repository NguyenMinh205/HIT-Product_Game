using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardDetailUI : MonoBehaviour
{
    [SerializeField] private Image markChoice;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    private ItemBase item;
    private RewardManager rewardManager;

    public Image MarkChoice => markChoice;
    public ItemBase Item => item;

    public void Init(ItemBase item)
    {
        this.item = item;
        itemIcon.sprite = item.icon;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        markChoice.color = Color.white;
        rewardManager = FindObjectOfType<RewardManager>();
        GetComponent<Button>().onClick.AddListener(() => rewardManager.SelectReward(this));
    }
}