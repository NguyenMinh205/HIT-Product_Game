using UnityEngine;

public class PoisonDamage : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public Sprite Icon { get; set; }
    private Player player;

    public PoisonDamage(float value, float duration)
    {
        Name = "poison_damage";
        Value = value;
        Duration = duration;
        Icon = UIEffectIcon.Instance.GetPoison;
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

        // Giả sử có cách lấy đối tượng Enemy hiện tại từ GamePlayController
        //Enemy enemy = GamePlayController.Instance.EnemyController.CurrentEnemy;
        //if (enemy != null)
        //{
        //    // Kiểm tra xem enemy đã có PoisonEffect chưa
        //    IBuffEffect existingPoison = enemy.GetActiveEffect("poison_effect");
        //    if (existingPoison != null)
        //    {
        //        // Cộng dồn duration
        //        existingPoison.Duration += Duration;
        //        Debug.Log($"Poison duration stacked. New duration: {existingPoison.Duration}");
        //    }
        //    else
        //    {
        //        // Áp dụng PoisonEffect mới lên enemy
        //        enemy.AddBuffEffect("poison_effect", Value, Duration);
        //        Debug.Log($"Applied PoisonEffect to enemy with {Value} damage for {Duration} turns.");
        //    }
        //}
        GamePlayController.Instance.PlayerController.CurrentPlayer.AddBuffEffect("poison_effect", Value, Duration);

        Duration--;
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