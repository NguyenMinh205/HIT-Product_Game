using UnityEngine;

public class PoisonGas : IBuffEffect //Hiệu ứng khí gas
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public PoisonGas(int value, int duration)
    {
        Name = "poison_gas";
       // Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) 
    {
        //player.AddBuffEffect("poison_effect", Value, Duration);
    }
    public void Remove(Player player) { }
}