using UnityEngine;

public class Vanish : IBuffEffect //Hiệu ứng bỏ chạy
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public Vanish(float value, float duration)
    {
        Name = "vanish";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}