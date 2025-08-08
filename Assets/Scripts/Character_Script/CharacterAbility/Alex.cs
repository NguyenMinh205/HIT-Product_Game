using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alex : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        player.AddBuffEffect("buff_shield_start_turn", 5, -1);
        ObserverManager<EventID>.PostEven(EventID.OnStartPlayerTurn);
    }

    public void StartSetupStat()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeMaxHP(10);
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCurHP(10);
        GamePlayController.Instance.PlayerController.listPerk.Add(new PerkInventory(PerkIconManager.Instance.Alex, "Alex Ability", "Your Health is increased by 10 when start game, Start each turn with 5 Block"));
    }
}
