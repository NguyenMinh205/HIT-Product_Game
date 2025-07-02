using UnityEngine;

public class BombEffect : IBuffEffect //Hiệu ứng bom
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public BombEffect(float value, float duration)
    {
        Name = "bomb_effect";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}