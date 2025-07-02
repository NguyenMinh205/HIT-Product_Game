using UnityEngine;

public interface IBuffEffect
{
    string Name { get; set; }
    BuffEffectType Type { get; set; }
    float Value { get; set; }
    float Duration { get; set; }
    void Apply(Player player);
    void Remove(Player player);
}

public enum BuffEffectType
{
    Turn_BasedEffects,
    ReactiveEffects,
}