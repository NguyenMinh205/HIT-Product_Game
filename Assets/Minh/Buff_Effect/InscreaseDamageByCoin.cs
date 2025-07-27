using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InscreaseDamageByCoin : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    
    private Enemy enemy;
    public InscreaseDamageByCoin(float value, float duration)
    {
        Name = "increase_damage_by_coin";
        Value = value;
        Duration = duration;
        Icon = UIEffectIcon.Instance.IncreaseDamageByCoin;
    }
    
    public void OnTakeCoin(object obj)
    {
        if (obj is int value)
        {
            enemy.DamageIncreased += value;
        }
        else
        {
            Debug.LogWarning("OnTakeCoin nhận không phải int!");
        }
    }

    public void Apply(Player player)
    {
        RegisterEvents();
    }

    public void ApplyEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        RegisterEvents();
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnTakeCoin, OnTakeCoin);
    }

    public void Remove(Player player)
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnTakeCoin, OnTakeCoin);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        UnregisterEvents();
    }

    public void UnregisterEvents()
    {
        UnregisterEvents();
    }
}
