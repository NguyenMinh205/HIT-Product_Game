using UnityEngine;

public class DoubleDamageEachTurn : IBuffEffect //Hiệu ứng x2 damage sau mỗi turn
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public DoubleDamageEachTurn(float value, float duration)
    {
        Name = "double_damage_each_turn";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}