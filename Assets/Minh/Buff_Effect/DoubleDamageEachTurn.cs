using UnityEngine;

public class DoubleDamageEachTurn : IBuffEffect //Hiệu ứng x2 damage sau mỗi turn (Boss)
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public DoubleDamageEachTurn(int value, int duration)
    {
        Name = "double_damage_each_turn";
        //Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}