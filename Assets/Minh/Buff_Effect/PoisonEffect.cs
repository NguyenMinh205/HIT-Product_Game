using UnityEngine;

public class PoisonEffect : IBuffEffect //Hiệu ứng nhận dame độc
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public PoisonEffect(int value, int duration)
    {
        Name = "poison_effect";
       // Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;

        ObserverManager<EffectManager>.AddDesgisterEvent(EffectManager.EffectPlayer, Effect);
    }
    public void Effect(object obj)
    {   
        if(Duration <= 0)
        {
            ObserverManager<EffectManager>.RemoveAddListener(EffectManager.EffectPlayer, Effect);
            return;
        }
        if (Duration > 0)
        {
            Duration--;


            EffectExecute(Value);
        }
    }
    public void EffectExecute(int damage)
    {
        GamePlayController.Instance.PlayerController.CurrentPlayer.ReceiveDamage(damage);
        GamePlayController.Instance.PlayerController.CurrentPlayer.Health.UpdateHp(GamePlayController.Instance.PlayerController.CurrentPlayer);
    }
    public void Apply(Player player) { }
    public void Remove(Player player) { }
}