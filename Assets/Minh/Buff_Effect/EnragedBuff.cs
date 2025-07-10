using UnityEngine;

public class EnragedEffect : IBuffEffect //Hiệu ứng tăng 100% tỉ lệ chí mạng khi máu dưới 30%
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Duration { get; set; }

    public EnragedEffect(int value, int duration)
    {
        Name = "enraged_effect";
        //Type = BuffEffectType.Turn_BasedEffects;
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player) 
    {
        if (player.Stats.currentHP <= player.Stats.maxHP * 0.3)
        {
            player.Stats.criticalChance = 1;
            return;
        } 
        Remove(player);  
    }
    public void Remove(Player player) 
    {
        player.Stats.criticalChance = 0;
    }
}