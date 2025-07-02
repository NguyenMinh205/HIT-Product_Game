using UnityEngine;

public class GoldenPower : IBuffEffect //Hiêu ứng gia tăng dame khi người chơi tăng vàng
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public GoldenPower(float value, float duration)
    {
        Name = "golden_power";
        Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}