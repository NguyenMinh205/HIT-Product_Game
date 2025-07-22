using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinDealDamage : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public AddCoinDealDamage(float value, float duration)
    {
        Name = "add_coin_deal_damage";
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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnDealDamage, OnDealDamage);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnDealDamage, OnDealDamage);
    }

    private void OnDealDamage(object param)
    {
        player.Stats.ChangeCoin((int)Value);
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
