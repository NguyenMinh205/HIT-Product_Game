using UnityEngine;

public class PoisonEffect : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    private Player player;

    public PoisonEffect(float value, float duration)
    {
        Name = "poison_effect";
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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
    }

    private void OnStartPlayerTurn(object param)
    {
        if (Duration <= 0)
        {
            Remove(player);
            return;
        }

        Duration--;
        player.ReceiveDamage((int)Value);
        Debug.Log($"Poison deals {Value} damage. Turns remaining: {Duration}");
    }
}