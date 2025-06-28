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
    [SerializeField] private List<IItemAction> actions = new List<IItemAction>();

    public void AddAction(IItemAction action)
    {
        actions.Add(action);
    }

    public void ExecuteActions(GameObject source, GameObject target, float value = 0)
    {
        foreach (var action in actions)
        {
            action.Execute(source, target, value);
        }
    }
}