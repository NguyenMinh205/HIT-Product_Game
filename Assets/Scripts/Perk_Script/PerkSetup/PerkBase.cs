using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Inventory/Perk")]
public class PerkBase : ScriptableObject
{
    public string id;
    public string perkName;
    public Sprite icon;
    [TextArea(1, 10)]
    public string description;

    [SerializeField] private IPerkAction action;

    public void Execute()
    {
        if (action == null)
        {
            action = PerkActionFactory.CreatePerkAction(id);
        }
        action.Execute();
        UiPerksList.Instance.AddPerks(icon);
    }
}