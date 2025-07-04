using UnityEngine;

public class MagnetClawEachTurns : IBuffEffect //Hiệu ứng thay móc thứ 2 thành móc nam châm
{
    public string Name { get; set; }
    public BuffEffectType Type { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    public MagnetClawEachTurns(float value, float duration)
    {
        Name = "magnet_claw_each_turns";
        Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}