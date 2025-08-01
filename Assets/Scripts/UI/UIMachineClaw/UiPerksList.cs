using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPerksList : Singleton<UiPerksList>
{
    [Header("Perk")]
    [SerializeField] private List<UiPerk> perks;
    [SerializeField] private RectTransform perkParent;
    [SerializeField] private UiPerk perkPrefabs;

    [Space]
    [Header("GameObject")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject listPerk;

    public void SetActivePerk(bool val)
    {
        background.SetActive(val);
        title.SetActive(val);
        listPerk.SetActive(val);
    }
    public void DisplayPerk(Sprite icon)
    {
        if (perkPrefabs == null) return;
        if (perkParent == null) return;

        if (perks.Count > 10) return;

        int index = perks.Count + 1;
        UiPerk newPerk = Instantiate(perkPrefabs, Vector2.one, Quaternion.identity, perkParent);
        newPerk.SetPos(index);
        newPerk.SetPerk(icon);
        perks.Add(newPerk);
    }
    public void AddPerks(Sprite icon)
    {
        DisplayPerk(icon);
    }
}
