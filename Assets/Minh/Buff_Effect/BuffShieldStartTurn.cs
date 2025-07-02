using UnityEngine;

public class BuffShieldStartTurn : IBuffEffect //Hiệu ứng tăng shield đầu turn
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public BuffShieldStartTurn(float value, float duration)
    {
        Name = "buff_shield_start_turn";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) 
    {
        player._CharacterStatModifier.ChangeShield(Value);
    }
    public void Remove(Player player) 
    {

    }
}