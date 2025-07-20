using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWater : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }

    private Coroutine moveCoroutine;

    public CreateWater()
    {
        Name = "create_water_in_box";
        Value = 0;
        Duration = 3;
        Icon = UIEffectIcon.Instance.CreateWater;
    }


    public void Apply(Player Player)
    {
    }

    public void Remove(Player player)
    {
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartEnemyTurn, ExecuteWater);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartEnemyTurn, ExecuteWater);
    }

    public void ApplyEnemy(Enemy enemy)
    {
        ObserverManager<WaterEffectType>.PostEven(WaterEffectType.CreateWater, enemy);
        RegisterEvents();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        UnregisterEvents();
    }

    public void ExecuteWater(object obj)
    {
        if(Duration > 0)
        {
            ObserverManager<WaterEffectType>.PostEven(WaterEffectType.ExecuteWater, null);
            Duration--;
        }
        else
        {
            RemoveEnemy(null);
            ObserverManager<EnemyEffect>.PostEven(EnemyEffect.Remove, this);
        }

    }
}
