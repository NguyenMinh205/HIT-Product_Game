using UnityEngine;

public static class BuffEffectFactory
{
    public static IBuffEffect CreateEffect(string effectName, float value, float duration)
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
                return new ThiefEffect(value, duration);

            case "create_water_in_box":
                return new CreateWater();

            //Buff/Effect for perk
            case "buff_shield_start_round":
                return new BuffShieldStartRound(value, duration);
            case "buff_strength_start_round":
                return new BuffStrengthStartRound(value, duration);
            case "add_spike_start_round":
                return new AddSpikeStartRound(value, duration);
            case "add_strength_take_damage":
                return new AddStrengthTakeDamage(value, duration);
            case "buff_health_per_coin":
                return new BuffHealthPerCoin(value, duration);
            case "add_spike_take_damage":
                return new AddSpikeTakeDamage(value, duration);
            case "buff_strength_by_items":
                return new BuffStrengthByItems(value, duration);
            case "buff_health_use_claw":
                return new BuffHealthUseClaw(value, duration);
            case "buff_health_end_round":
                return new BuffHealthEndRound(value, duration);
            case "buff_coin_end_round":
                return new BuffCoinEndRound(value, duration);
            case "add_coin_deal_damage":
                return new AddCoinDealDamage(value, duration);
            case "add_poison_on_enemy":
                return new ApplyPoisonEffect(value, duration);

            default:
                Debug.LogWarning($"Hiệu ứng {effectName} không được hỗ trợ!");
                return null;
        }
    }
}