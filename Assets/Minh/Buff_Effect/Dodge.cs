using UnityEngine;

public class Dodge : IBuffEffect //Hiệu ứng né đòn
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public Dodge(int value, int duration)
    {
        Name = "dodge";
        //Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}