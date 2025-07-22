using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpikeStartRound : IBuffEffect
{
    public string Name { get; set ; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public AddSpikeStartRound(float value, float duration)
    {
        Name = "add_spike_start_round";
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player)
    {
        this.player = player;
        RegisterEvents();
    }

    public void ApplyEnemy(Enemy enemy)
    {
        
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartRound, OnStartRound);
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartRound, OnStartRound);
    }

    private void OnStartRound(object param)
    {
        if (player != null)
        {
            player.AddBuffEffect("thorns_damage", 3, -1);
        }
    }
}
