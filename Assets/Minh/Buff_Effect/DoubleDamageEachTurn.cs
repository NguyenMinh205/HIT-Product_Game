using UnityEngine;

public class DoubleDamageEachTurn : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;
    private Enemy enemy;

    public DoubleDamageEachTurn(float value, float duration)
    {
        Name = "double_damage_each_turn";
        Value = value;
        Duration = duration;
        Icon = UIEffectIcon.Instance.x2DamageTurn;
    }

    public void Apply(Player player)
    {
        this.player = player;
        RegisterEvents();
    }
    
    public void Apply(Enemy enemy)
    {
        this.enemy = enemy;
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

        // Logic: Nhân đôi sát thương của player
        // Giả sử CharacterStatModifier có phương thức ChangeDamage để điều chỉnh sát thương
        //player._CharacterStatModifier.ChangeDamage(player._CharacterStatModifier.Stats.damage * (Value > 0 ? Value : 2f));
        //Debug.Log($"Damage doubled to {player._CharacterStatModifier.Stats.damage}. Duration: {(Duration == -1 ? "Permanent" : Duration.ToString())}");

        enemy.Damage *= 2; // Giả sử Enemy có thuộc tính Damage để điều chỉnh sát thương

        if (Duration != -1)
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