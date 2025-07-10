using UnityEngine;

public class Vanish : IBuffEffect //Hiệu ứng bỏ chạy (Quái trộm)
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public Vanish(int value, int duration)
    {
        Name = "vanish";
        //Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}