using UnityEngine;

public class EnragedEffect : IBuffEffect //Hiệu ứng tăng 100% tỉ lệ chí mạng khi máu dưới 30%
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public EnragedEffect(float value, float duration)
    {
        Name = "enraged_effect";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}