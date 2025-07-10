using UnityEngine;

public class BombEffect : IBuffEffect //Hiệu ứng bom
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public BombEffect(int value, int duration)
    {
        Name = "bomb_effect";
       // Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}