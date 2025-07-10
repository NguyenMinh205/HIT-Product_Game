using UnityEngine;

public static class BuffEffectFactory
{
    public static IBuffEffect CreateEffect(string effectName, int value, int duration)
    {
        switch (effectName.ToLower())
        {
            // Turn_BasedEffects
            case "buff_shield_start_turn":
                return new BuffShieldStartTurn(value, duration);
            case "bomb_effect":
                return new BombEffect(value, duration);
            case "double_damage_each_turn":
                return new DoubleDamageEachTurn(value, duration);
            case "enraged_effect":
                return new EnragedEffect(value, duration);
            case "magnet_claw_each_turns":
                return new MagnetClawEachTurns(value, duration);
            case "poison_effect":
                return new PoisonEffect(value, duration);
            case "poison_gas":
                return new PoisonGas(value, duration);
            case "retain_block":
                return new RetainBlock(value, duration);
            case "vanish":
                return new Vanish(value, duration);
            // ReactiveEffects
            case "counter_attack":
                return new CounterAttack(value, duration);
            case "dodge":
                return new Dodge(value, duration);
            case "golden_power":
                return new GoldenPower(value, duration);
            case "lifesteal":
                return new Lifesteal(value, duration);
            case "poison_damage":
                return new PoisonDamage(value, duration);
            case "thorns_damage":
                return new ThornsDamage(value, duration);
            case "thief":
                return new Thief(value, duration);

            default:
                Debug.LogWarning($"Hiệu ứng {effectName} không được hỗ trợ!");
                return null;
        }
    }
}