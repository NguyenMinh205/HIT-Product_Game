using System.Collections;
using System.Collections.Generic;
using TranDuc;
using Unity.VisualScripting;
using UnityEngine;

public class UiPerksList : Singleton<UiPerksList>
{
    [Header("Perk")]
    [SerializeField] private RectTransform perkParent;
    [SerializeField] private UiPerk perkPrefabs;
    [Space]
    [Header("GameObject")]
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject listPerk;

    public void SetActivePerk(bool val)
    {
        title.SetActive(val);
        listPerk.SetActive(val);
        if (val == true)
        {
            DisplayPerk(DataManager.Instance.GameData.Player.perks);
        }
        else
        {
            ClearPerks();
        }    
    }
    public void DisplayPerk(List<PerkInventory> listPerk)
    {
        foreach (PerkInventory perkIcon in listPerk)
        {
            UiPerk perk = Instantiate(perkPrefabs, this.transform.position, Quaternion.identity, perkParent);
            perk.SetPerk(perkIcon.icon, perkIcon.perkName, perkIcon.description);
        }
    }

    public void ClearPerks()
    {
        foreach (Transform perk in perkParent)
        {
            Destroy(perk.gameObject);
        }
    }
}

[System.Serializable]
public class PerkInventory
{
    public Sprite icon;
    public string perkName;
    public string description;

    public PerkInventory(Sprite icon, string perkName, string description)
    {
        this.icon = icon;
        this.perkName = perkName;
        this.description = description;
    }
}
