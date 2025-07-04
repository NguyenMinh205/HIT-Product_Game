using UnityEngine;

public class RetainBlock : IBuffEffect //Hiệu ứng giữ lại khiên sau mỗi turn (Boss)
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public RetainBlock(float value, float duration)
    {
        Name = "retain_block";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}