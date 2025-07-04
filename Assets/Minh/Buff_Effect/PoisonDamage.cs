using UnityEngine;

public class PoisonDamage : IBuffEffect //Hiệu ứng đánh gây hiệu ứng dame độc
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public PoisonDamage(float value, float duration)
    {
        Name = "poison_damage";
        Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
} 