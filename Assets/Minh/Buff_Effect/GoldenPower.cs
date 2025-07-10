using UnityEngine;

public class GoldenPower : IBuffEffect //Hiêu ứng gia tăng dame khi người chơi tăng vàng (Boss cuối)
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public GoldenPower(int value, int duration)
    {
        Name = "golden_power";
       // Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}