using UnityEngine;

public class Lifesteal : IBuffEffect //Hiệu ứng hút máu
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public Lifesteal(int value, int duration)
    {
        Name = "lifesteal";
        //Type = BuffEffectType.ReactiveEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) 
    {
        player._CharacterStatModifier.ChangeBloodsuckingRate(Value);
    }
    public void Remove(Player player) { }
}
