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
        if (Duration <= 0)
        {
            Remove(player);
            return;
        }

        if (player != null)
        {
            player.IsDodge = true;
            Debug.Log("Player dodged attack! No damage taken.");
        }
        else if (enemy != null)
        {
            // Log cho Enemy (giả sử Enemy không dùng IsDodge)
            Debug.Log($"Enemy {enemy.name} dodged attack! No damage taken.");
        }

        Duration--;
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