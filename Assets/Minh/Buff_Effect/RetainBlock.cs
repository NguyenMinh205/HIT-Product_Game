using UnityEngine;

public class RetainBlock : IBuffEffect //Hiệu ứng giữ lại khiên sau mỗi turn (Boss)
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public RetainBlock(int value, int duration)
    {
        Name = "retain_block";
        //Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}