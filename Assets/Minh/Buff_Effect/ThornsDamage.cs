using UnityEngine;

public class ThornsDamage : IBuffEffect //Hiệu ứng gai phản dame
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public ThornsDamage(int value, int duration)
    {
        Name = "thorns_damage";
        //Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}