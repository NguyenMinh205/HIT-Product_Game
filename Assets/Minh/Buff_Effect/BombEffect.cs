using UnityEngine;
using System.Collections.Generic;

public class BombEffect : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private object target; // Có thể là Player hoặc List<Enemy>
    private bool isAppliedByPlayer; // True nếu Player áp dụng, False nếu Enemy áp dụng

    public BombEffect(float value, float duration)
    {
        Name = "bomb_effect";
        Value = value; // Sát thương bom (30 cho Player gây, 45 cho Enemy gây)
        Duration = duration;
        Icon = UIEffectIcon.Instance.Explosion2;
    }

    public void Apply(Player player)
    {
        this.target = player;
        this.isAppliedByPlayer = false; // Enemy áp dụng lên Player
        RegisterEvents();
    }

    public void Apply(List<Enemy> enemies)
    {
        this.target = enemies;
        this.isAppliedByPlayer = true; // Player áp dụng lên các Enemy
        RegisterEvents();
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RegisterEvents()
    {
        // Đăng ký sự kiện dựa trên đối tượng chịu hiệu ứng
        if (target is Player)
        {
            ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartEnemyTurn, OnStartEnemyTurn);
        }
        else if (target is List<Enemy>)
        {
            ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
        }
    }

    public void UnregisterEvents()
    {
        // Hủy đăng ký sự kiện tương ứng
        if (target is Player)
        {
            ObserverManager<EventID>.RemoveAddListener(EventID.OnStartEnemyTurn, OnStartEnemyTurn);
        }
        else if (target is List<Enemy>)
        {
            ObserverManager<EventID>.RemoveAddListener(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
        }
    }

    private void OnStartPlayerTurn(object param)
    {
        if (Duration != -1 && Duration <= 0)
        {
            if (isAppliedByPlayer && target is List<Enemy> enemies)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy != null)
                    {
                        enemy.ReceiverDamage(30);
                        Debug.Log($"Bomb exploded, dealing 30 damage to {enemy.name}!");
                    }
                }
            }
            Remove(null);
            return;
        }

        Debug.Log($"Bomb ticking on {(target is Player ? "Player" : "Enemies")}... Duration: {(Duration == -1 ? "Permanent" : Duration.ToString())}");

        if (Duration != -1)
        {
            Duration--;
        }
    }

    private void OnStartEnemyTurn(object param)
    {
        if (Duration != -1 && Duration <= 0)
        {
            if (!isAppliedByPlayer && target is Player player)
            {
                player.ReceiveDamage(45);
                Debug.Log($"Bomb exploded on Player, dealing 45 damage!");
            }
            Remove(target as Player);
            return;
        }

        Debug.Log($"Bomb ticking on {(target is Player ? "Player" : "Enemies")}... Duration: {(Duration == -1 ? "Permanent" : Duration.ToString())}");

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