using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private ItemBase data;
    [SerializeField] private TextMeshProUGUI numOfItem;
    public ItemBase Data => data;
    public TextMeshProUGUI NumOfItem => numOfItem;

    public void Init(ItemBase data, Action<ItemBase> action, int quantity = 1)
    {
        this.data = data;
        iconImage.sprite = data.icon;
        numOfItem.text = quantity.ToString();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => action(this.data));
    }
}