using UnityEngine;

public class CounterAttack : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; } 
    public float Duration { get; set; }

    public Sprite Icon { get; set; }
    private Player player;
    private Enemy enemy;

    public CounterAttack(float value, float duration)
    {
        Name = "counter_attack";
        Value = value;
        Duration = duration;
        
        Icon = UIEffectIcon.Instance.CounterAttack;
    }

    public void Apply(Player player)
    {
        this.player = player;
        this.enemy = null;
        RegisterEvents();
    }

    public void Apply(Enemy enemy)
    {
        this.enemy = enemy;
        this.player = null;
        RegisterEvents();
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnReceiverDamage, OnReceiverDamage);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnReceiverDamage, OnReceiverDamage);
    }

    private void OnReceiverDamage(object param)
    {
        if (param is Player player)
        {
            if (Value <= 0)
            {
                Remove(player);
                return;
            }
            player.IsCounterAttack = true;
        }
        else if (param is Enemy enemy)
        {
            if (Value <= 0)
            {
                RemoveEnemy(enemy);
                return;
            }
            enemy.IsCounterAttack = true;
        }

        if (Duration == -1)
        {
            Value--;
        }
        else
        {
            Duration--;
        }
    }

    public void ApplyEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        RegisterEvents();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        UnregisterEvents();
    }
}