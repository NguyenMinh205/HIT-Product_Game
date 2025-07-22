using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStrengthByItems : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public BuffStrengthByItems(float value, float duration)
    {
        Name = "buff_strength_by_items";
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player)
    {
        this.player = player;
        RegisterEvents();
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnClawsEmpty, CheckItemsEmpty);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnClawsEmpty, CheckItemsEmpty);
    }

    private void CheckItemsEmpty(object param)
    {
        if (GamePlayController.Instance.ItemController.IsPickupItemSuccess == false)
        {
            player.Stats.ChangeStrength(Value);
        }
    }

    public void ApplyEnemy(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }
}
