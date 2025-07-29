using UnityEngine;

public class Dodge : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; } // Không dùng, giữ để tương thích IBuffEffect
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;
    private Enemy enemy;

    public Dodge(float value, float duration)
    {
        Name = "dodge";
        Value = value; // Không dùng
        Duration = duration;
        Icon = UIEffectIcon.Instance.Dodge; 
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
        if(param is Player player)
        {
            if (Value <= 0)
            {
                Remove(player);
                return;
            }
            player.IsDodge = true;
        }
        else if(param is Enemy enemy)
        {
            if (Value <= 0)
            {
                RemoveEnemy(enemy);
                return;
            }
            enemy.IsDodge = true;
        }

        if(Duration == -1)
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