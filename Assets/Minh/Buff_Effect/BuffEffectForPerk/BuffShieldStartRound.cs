using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShieldStartRound : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public BuffShieldStartRound(float value, float duration)
    {
        Name = "buff_shield_start_round";
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
            player.Stats.ChangeShield(Value);
        }
    }
}
