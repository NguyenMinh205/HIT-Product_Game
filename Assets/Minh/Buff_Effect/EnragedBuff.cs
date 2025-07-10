using UnityEngine;

public class EnragedEffect : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    private Player player;
    private float percentIncrease = 0;

    public EnragedEffect(float value, float duration)
    {
        Name = "enraged_effect";
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
        if (player.Stats.CurrentHP <= player.Stats.MaxHP * 0.3f)
        {
            percentIncrease = 1 - player.Stats.CriticalChance;
            player.Stats.ChangeCriticalChance(percentIncrease);
            Debug.Log($"Critical chance set to 100%. Turns remaining: {Duration}");
        }
        else
        {
            player.Stats.ChangeCriticalChance(-percentIncrease);
            Remove(player);
        }
    }
}