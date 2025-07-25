using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum IDPerkUI
{
    AddPerk,
    RemovePerk,
}
public class UiPerksList : MonoBehaviour
{
    [SerializeField] private List<UiPerk> perks;
    [SerializeField] private List<PerkBase> perkBases;
    private void Awake()
    {
        ObserverManager<IDPerkUI>.AddDesgisterEvent(IDPerkUI.AddPerk, AddPerks);
    }
    private void OnDisable()
    {
        ObserverManager<IDPerkUI>.RemoveAddListener(IDPerkUI.AddPerk, AddPerks);
    }

    public void AddPerks(object obj)
    {
        Debug.Log("Add Perk");
        if (obj is PerkBase perk)
        {
            if(perkBases.Contains(perk))
            {
                SetUIPerkText(perk);
                return;
            }
            else
            {
                Debug.Log("Add Perk to UI");
                perkBases.Add(perk);
                SetUIPerk(perk);
            }
        }
    }
    public void SetUIPerk(PerkBase perk)
    {
        foreach (UiPerk uiPerk in perks)
        {
            if (uiPerk.Perk == null)
            {
                uiPerk.SetPerk(perk);
                return;
            }
        }
    }
    public void SetUIPerkText(PerkBase perk)
    {
        foreach (UiPerk uiPerk in perks)
        {
            if (uiPerk.Perk == perk)
            {
                uiPerk.SetTextPerk();
                return;
            }
        }
    }
}
