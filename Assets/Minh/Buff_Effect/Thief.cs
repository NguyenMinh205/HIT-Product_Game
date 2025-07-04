using UnityEngine;

public class Thief : IBuffEffect //Hiệu ứng ăn trộm (Quái trộm)
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public Thief(float value, float duration)
    {
        Name = "thief";
        Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}