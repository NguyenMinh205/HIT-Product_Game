using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpikeTakeDamage : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;
    private int indexBuff;

    public AddSpikeTakeDamage(float value, float duration)
    {
        Name = "add_spike_take_damage";
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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnTakeDamage, OnTakeDamage);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnTakeDamage, OnTakeDamage);
    }

    private void OnTakeDamage(object param)
    {
        player.AddBuffEffect("thorns_damage", Value, Duration);
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
