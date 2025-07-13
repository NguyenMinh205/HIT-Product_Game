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
        iconImage.SetNativeSize();
        iconImage.rectTransform.sizeDelta *= 0.75f;
        numOfItem.text = quantity.ToString();

        //if (GameManager.Instance.CurrentRoom == )
        //if (data.isUpgraded || data.upgradedItem == null)
        //{
        //    Color color = iconImage.color;
        //    color.a = 200f / 255f;
        //    iconImage.color = color;
        //    GetComponent<Button>().interactable = false;
        //}
        //else
        //{
        //    Color color = iconImage.color;
        //    color.a = 1f;
        //    iconImage.color = color;
        //    GetComponent<Button>().interactable = true;
        //}

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => action(this.data));
    }
}