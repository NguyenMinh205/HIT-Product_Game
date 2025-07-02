using UnityEngine;

public class ThornsDamage : IBuffEffect //Hiệu ứng gai phản dame
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public ThornsDamage(float value, float duration)
    {
        Name = "thorns_damage";
        Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}