using UnityEngine;

public class PoisonDamage : IBuffEffect //Hiệu ứng đánh gây hiệu ứng dame độc
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public PoisonDamage(int value, int duration)
    {
        Name = "poison_damage";
        //Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
} 