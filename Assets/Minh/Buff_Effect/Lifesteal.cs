using UnityEngine;

public class Lifesteal : IBuffEffect //Hiệu ứng hút máu
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public Lifesteal(float value, float duration)
    {
        Name = "lifesteal";
        Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}
