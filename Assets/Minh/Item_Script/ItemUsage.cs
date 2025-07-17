using UnityEngine;
using System.Collections.Generic;

public class ItemUsage : MonoBehaviour
{
    public void UseItem(string itemId, Player player, Enemy target = null)
    {
        ItemBase item = ItemDatabase.Instance.GetItemById(itemId);
        if (item != null)
        {
            item.ExecuteAction(player, target);
            if (item.Action is AttackItem or AttackWithBuff or AttackWithEffect)
            {
                ObserverManager<IDStateAnimationPlayer>.PostEven(IDStateAnimationPlayer.Attack, null);
            }
            else
            {
                ObserverManager<IDStateAnimationPlayer>.PostEven(IDStateAnimationPlayer.Buff, null);
            }
        }
    }

    public void UseMultipleItems(List<string> itemIds, Player player, Enemy target = null)
    {
        foreach (string id in itemIds)
        {
            UseItem(id, player, target);
        }
    }
}