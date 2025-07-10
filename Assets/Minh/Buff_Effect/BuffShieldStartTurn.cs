using UnityEngine;

public class BuffShieldStartTurn : IBuffEffect //Hiệu ứng tăng shield đầu turn
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public BuffShieldStartTurn(int value, int duration)
    {
        Name = "buff_shield_start_turn";
       // Type = BuffEffectType.Turn_BasedEffects;
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