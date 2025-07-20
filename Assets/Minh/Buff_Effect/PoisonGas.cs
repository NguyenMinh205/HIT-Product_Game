using UnityEngine;

public class PoisonGas : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public Sprite Icon { get; set; }
    private Player player;

    
    public PoisonGas(float value, float duration)
    {
        Name = "poison_gas";
        Value = value; // Sát thương của PoisonEffect
        Duration = duration;
        Icon = UIEffectIcon.Instance.PosionGas; 
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
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartEnemyTurn, OnStartPlayerTurn);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartEnemyTurn, OnStartPlayerTurn);
    }

    private void OnStartPlayerTurn(object param)
    {
        if (Duration != -1 && Duration <= 0)
        {
            Remove(player);
            return;
        }
        
        GamePlayController.Instance.PlayerController.CurrentPlayer.AddBuffEffect("poison_effect", Value, Value);
        Debug.Log($"Applied PoisonEffect to player with {Value} damage for {Value} turns.");

        if (Duration != -1)
        {
            Duration--;
        }
    }

    public void ApplyEnemy(Enemy enemy)
    {
        RegisterEvents();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        UnregisterEvents();
    }
}