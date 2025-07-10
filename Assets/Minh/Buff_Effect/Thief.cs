using UnityEngine;

public class Thief : IBuffEffect //Hiệu ứng ăn trộm (Quái trộm)
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public Thief(int value, int duration)
    {
        Name = "thief";
        //Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}