using UnityEngine;
using UnityEngine.UIElements.Experimental;

public static class ItemActionFactory
{
    public static IItemAction CreateItemAction(string id)
    {
        switch (id)
        {
            case "Atk01":
                return new Dagger();
            case "Atk02":
                return new SmallSword();
            case "Atk06":
                return new Sickle();
            case "Atk07":
                return new MagicWand();
            case "Atk09":
                return new SpikedMace();
            case "Atk10":
                return new GreatSword();
            case "Atk11":
                return new BattleAxe();
            case "Atk12":
                return new Warhammer();
            case "Atk13":
                return new PlasticKnife();
            case "Atk15":
                return new PoisonDagger();
            case "Atk16":
                return new Paperclips();
            case "Atk17":
                return new LuckyStick();
            case "Atk23":
                return new LuckyStick();
            case "Def02":
                return new SpikeyShield();
            case "Def03":
                return new BigShield();
            case "Def04":
                return new BodyArmor();
            case "Def05":
                return new TowerShield();
            case "Def06":
                return new Helmet();
            case "Def08":
                return new SmallShield();
            case "Def09":
                return new PlasticShield();
            case "Def11":
                return new MetalShield();
            case "Buf04":
                return new RingOfStrength();
            case "Buf08":
                return new AmuletOfStrength();
            case "Buf11":
                return new StrengthPoison();
            case "Buf13":
                return new HealingFlask();
            case "Buf17":
                return new PoisonGrenade();
            case "Buf19":
                return new VitaminPill();
            case "Buf21":
                return new EnergyDrink();
            default:
                Debug.LogWarning($"Không tìm thấy hành động cho ID: {id}");
                return null;
        }
    }
}