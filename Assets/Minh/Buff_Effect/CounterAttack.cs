using UnityEngine;
using UnityEngine.UIElements;

public class CounterAttack : IBuffEffect //Hiệu ứng phản đòn
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public CounterAttack(float value, float duration)
    {
        Name = "counter_attack";
        Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player)
    {
        Duration--;
        if (Duration <= 0)
        {
            Remove(player);
        }
    }
    public void Remove(Player player) 
    {

    }
}