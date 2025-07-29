using UnityEngine;
using System.Collections.Generic;

public class ItemUsage : MonoBehaviour
{
    public void UseItem(string itemId, Player player, Enemy target = null, List<Enemy> targets = null)
    {
        ItemBase item = ItemDatabase.Instance.GetItemById(itemId);
        if (item != null)
        {
            item.ExecuteAction(player, target, targets);
            if (item.Action is AttackItem or AttackWithBuff or AttackWithEffect)
            {
                AudioManager.Instance.PlayPlayerAttack();
                ObserverManager<IDStateAnimationPlayer>.PostEven(IDStateAnimationPlayer.Attack, null);
            }
            else
            {
                AudioManager.Instance.PlayPlayerBuff();
                ObserverManager<IDStateAnimationPlayer>.PostEven(IDStateAnimationPlayer.Buff, null);
            }
        }
    }

    public void UseMultipleItems(List<string> itemIds, Player player, Enemy target = null, List<Enemy> targets = null)
    {
        foreach (string id in itemIds)
        {
            UseItem(id, player, target, targets);
        }
    }
}