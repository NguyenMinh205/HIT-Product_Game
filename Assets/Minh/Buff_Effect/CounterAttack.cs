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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnTakeDamage, OnTakeDamage);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnTakeDamage, OnTakeDamage);
    }

    private void OnTakeDamage(object param)
    {
        if (Value <= 0)
        {
            Remove(player);
            return;
        }

        if (player != null)
        {
            player.IsCounterAttack = true;
            Debug.Log("Player CounterAttack triggered! Ready to counter damage.");
        }
        else if (enemy != null)
        {
            //enemy.IsCounterAttack = true;
            Debug.Log($"Enemy {enemy.name} CounterAttack triggered! Ready to counter damage.");
        }

        Value--;
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