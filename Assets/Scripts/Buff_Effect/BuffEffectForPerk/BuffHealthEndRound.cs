using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHealthEndRound : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public BuffHealthEndRound(float value, float duration)
    {
        Name = "buff_health_end_round";
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
        player.Stats.ChangeCurHP(Value);
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
