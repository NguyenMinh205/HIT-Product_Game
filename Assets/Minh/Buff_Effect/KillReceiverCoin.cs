using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillReceiverCoin : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; } 
    public float Duration { get; set; }
    public Sprite Icon { get; set; }

    public KillReceiverCoin( float value, float duration)
    {
        Name = "kill_receiver_coin";
        Value = value;
        Duration = duration;
        Icon = UIEffectIcon.Instance.Coin;
    }
    public void Apply(Player player)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyEnemy(Enemy enemy)
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnEnemyDead, OnDead);
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnEnemyDead, OnDead);
    }

    public void OnDead(object obj)
    {
        // Khi Enemy chết, sẽ nhận coin
        // Thực hiện logic nhận coin ở đây
        
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCoin((int)Value);
    }

    public void Remove(Player player)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        RegisterEvents();
    }

    public void UnregisterEvents()
    {
        UnregisterEvents();
    }
}
