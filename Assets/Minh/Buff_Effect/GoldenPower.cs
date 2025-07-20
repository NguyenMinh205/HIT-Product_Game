using UnityEngine;

public class GoldenPower : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;

    public GoldenPower(float value, float duration)
    {
        Name = "golden_power";
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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnGoldChanged, OnGoldChanged);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnGoldChanged, OnGoldChanged);
    }

    private void OnGoldChanged(object param)
    {
        if (Duration <= 0)
        {
            Remove(player);
            return;
        }

        float goldAmount = (float)param;
        //player._CharacterStatModifier.ChangeDamage(Value * goldAmount);
        Debug.Log($"Damage increased by {Value * goldAmount}. Turns remaining: {Duration}");
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