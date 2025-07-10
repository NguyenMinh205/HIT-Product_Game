using UnityEngine;

public class Lifesteal : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    private Player player;

    public Lifesteal(float value, float duration)
    {
        Name = "lifesteal";
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
        if (Duration <= 0)
        {
            Remove(player);
            return;
        }

        int damageDealt = (int)param;
        int healAmount = (int)(damageDealt * Value);
        //player._CharacterStatModifier.Heal(healAmount);
        Debug.Log($"Lifesteal heals for {healAmount}. Turns remaining: {Duration}");
    }
}