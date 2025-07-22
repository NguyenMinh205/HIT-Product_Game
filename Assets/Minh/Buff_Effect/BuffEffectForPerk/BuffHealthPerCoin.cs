using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHealthPerCoin : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    public Sprite Icon { get; set; }
    private Player player;
    private int indexBuff;

    public BuffHealthPerCoin(float value, float duration)
    {
        Name = "buff_health_per_coin";
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
        int index =  Mathf.Min((int)(player.Stats.Coin / 10), 10);
        player.Stats.ChangeMaxHP(Value * (index - indexBuff));
        player.Stats.ChangeCurHP(Value * (index - indexBuff));
        indexBuff = index;
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
