using UnityEngine;

public class PoisonEffect : IBuffEffect //Hiệu ứng nhận dame độc
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public PoisonEffect(float value, float duration)
    {
        Name = "poison_effect";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}