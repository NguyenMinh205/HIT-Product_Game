using UnityEngine;

public class ThornsDamage : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public ThornsDamage(float value, float duration)
    {
        Name = "thorns_damage";
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

        int damageTaken = (int)param;
        int reflectDamage = (int)(damageTaken * Value);
        // Giả sử có cách phản sát thương lại kẻ địch
        Debug.Log($"Thorns reflect {reflectDamage} damage. Turns remaining: {Duration}");
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