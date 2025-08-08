using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violet : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        player.AddBuffEffect("poison_damage", 1, -1);
    }

    public void StartSetupStat()
    {
        GamePlayController.Instance.PlayerController.listPerk.Add(new PerkInventory(PerkIconManager.Instance.Violet, "Violet Ability", "Each time you apply Poison to an enemy, you apply 1 more poison. Each attack applies 1 Poison"));
    }
}
