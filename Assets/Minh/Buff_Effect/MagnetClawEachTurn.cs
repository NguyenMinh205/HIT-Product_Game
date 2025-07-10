using UnityEngine;

public class MagnetClawEachTurns : IBuffEffect //Hiệu ứng thay móc thứ 2 thành móc nam châm
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public MagnetClawEachTurns(int value, int duration)
    {
        Name = "magnet_claw_each_turns";
        //Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) { }
    public void Remove(Player player) { }
}