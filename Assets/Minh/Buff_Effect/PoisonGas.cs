using UnityEngine;

public class PoisonGas : IBuffEffect //Hiệu ứng khí gas
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public PoisonGas(float value, float duration)
    {
        Name = "poison_gas";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}