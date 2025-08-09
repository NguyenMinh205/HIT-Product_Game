using UnityEngine;

public class PoisonEffect : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public Sprite Icon { get; set; }
    private Player player;
    private Enemy enemy;

    public PoisonEffect(float value, float duration)
    {
        Name = "poison_effect";
        Value = value;
        Duration = duration;
        Debug.Log("Set Icon For PoisonEffect");
        Icon = UIEffectIcon.Instance.Posion; 
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
            if(player != null) Remove(player);
            return;
        }

        Duration--;
        player.Stats.ChangeCurHP(-Value);
        player.UpdateHpUI();

        Debug.Log($"Poison deals {Value} damage. Turns remaining: {Duration}");
    }
    private void OnStartEnemyTurn(object obj)
    {
        if (Duration <= 0)
        {
            if (enemy != null) RemoveEnemy(enemy);
            return;
        }

        Duration--;
        enemy.HP -= (int)Value;

        if (enemy.HP < 0)
            enemy.HP = 0;

        ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Hit, this);
        enemy.Health.UpdateHp(enemy);

        Debug.Log($"Poison deals {Value} damage. Turns remaining: {Duration}");
    }
    public void ApplyEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartEnemyTurn, OnStartEnemyTurn);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartEnemyTurn, OnStartEnemyTurn);
    }
}
