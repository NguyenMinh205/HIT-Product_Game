using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CompendiumItemUI : MonoBehaviour
{
    public Image iconImage;
    private ScriptableObject data;

    public void Setup(ScriptableObject data)
    {
        this.data = data;

        if (data is ItemBase item)
        {
            iconImage.sprite = item.icon;
        }
        else if (data is PerkBase perk)
        {
            iconImage.sprite = perk.icon;
        }

        GetComponent<Button>().onClick.AddListener(OnItemClick);
    }

    private void OnItemClick()
    {
        CompendiumManager.Instance.ShowDetail(data);
    }
}
