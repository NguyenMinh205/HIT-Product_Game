using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPoisonEffect : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public ApplyPoisonEffect(float value, float duration)
    {
        Name = "add_poison_on_enemy";
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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartRound, OnStartRound);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartRound, OnStartRound);
    }

    private void OnStartRound(object param)
    {
        foreach (Enemy enemy in GamePlayController.Instance.EnemyController.ListEnemy)
        {
            if (enemy != null)
            {
                ApplyEnemy(enemy);
            }
        }
    }

    public void ApplyEnemy(Enemy enemy)
    {
        enemy.AddBuffEffect("poison_effect", Value, Duration);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }
}
