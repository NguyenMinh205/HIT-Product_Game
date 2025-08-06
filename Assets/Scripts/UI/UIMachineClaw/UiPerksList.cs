using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UiPerksList : Singleton<UiPerksList>
{
    [Header("Perk")]
    [SerializeField] private List<UiPerk> perks;
    [SerializeField] private RectTransform perkParent;
    [SerializeField] private UiPerk perkPrefabs;
    public List<UiPerk> Perks => perks;
    [Space]
    [Header("GameObject")]
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject listPerk;

    public void SetActivePerk(bool val)
    {
        title.SetActive(val);
        listPerk.SetActive(val);
    }
    public void DisplayPerk(UiPerk uiPerk ,Sprite icon)
    {
        if (perkPrefabs == null) return;
        if (perkParent == null) return;

        if (perks.Count > 10) return;

        int index = perks.Count + 1;

        if (icon == null) return;
        foreach(UiPerk perk in perks)
        {
            if (perk.Icon.sprite == icon)
                return;
        }

        if (uiPerk == null) return;

        uiPerk.SetPos(index);
        uiPerk.SetPerk(icon);
    }
    public void AddPerks(PerkBase perkBase)
    {
        UiPerk newPerk = Instantiate(perkPrefabs, Vector2.one, Quaternion.identity, perkParent);
        DisplayPerk(newPerk, perkBase.icon);

        newPerk.SetPerk(perkBase.icon, perkBase.name, perkBase.description);
        perks.Add(newPerk);
    }
    public void AddPerks(Sprite icon = null, string perkName = null, string description = null)
    {
        UiPerk newPerk = Instantiate(perkPrefabs, Vector2.one, Quaternion.identity, perkParent);
        DisplayPerk(newPerk, icon);

        newPerk.SetPerk(icon, perkName, description);
        perks.Add(newPerk);
    }
}
