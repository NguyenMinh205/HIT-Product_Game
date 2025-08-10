using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStrengthTakeDamage : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public AddStrengthTakeDamage(float value, float duration)
    {
        Name = "add_strength_take_damage";
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
        RegisterEvents();
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnTakeDamage, OnTakeDamage);
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        UnregisterEvents();
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnTakeDamage, OnTakeDamage);
    }

    public void OnTakeDamage(object param)
    {
        if (player != null)
        {
            player.Stats.ChangeStrength(1);
        }
    }
}
