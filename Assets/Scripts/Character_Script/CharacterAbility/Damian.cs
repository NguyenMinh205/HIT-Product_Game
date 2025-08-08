using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Damian : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        
    }

    public void StartSetupStat()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeMaxHP(10);
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCurHP(10);
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeDamageAbsorb(0.15f);
        GamePlayController.Instance.PlayerController.listPerk.Add(new PerkInventory(PerkIconManager.Instance.Damian, "Damian Ability", "Cannot block damage, heals itself for 15 % of all damage dealt to enemies"));
    }
}
