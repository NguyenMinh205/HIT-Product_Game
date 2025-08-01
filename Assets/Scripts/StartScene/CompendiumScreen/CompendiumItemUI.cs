using UnityEngine;
using UnityEngine.UI;

public class CompendiumItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private ScriptableObject data;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
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

    private void OnDrawGizmosSelected()
    {
        Setup();
    }
}
