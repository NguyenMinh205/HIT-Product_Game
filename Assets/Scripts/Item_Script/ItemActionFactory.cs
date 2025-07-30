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
            case "Atk03":
                return new DarkSword();
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
            case "Atk14":
                return new TickingBomb();
            case "Atk15":
                return new PoisonDagger();
            case "Atk16":
                return new Paperclips();
            case "Atk17":
                return new LuckyStick();
            case "Atk19":
                return new Syringe();
            case "Atk20":
                return new Shuriken();
            case "Atk21":
                return new Thermometer();
            case "Atk23":
                return new DoubleBladedSword();
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
            case "Buf01":
                return new Magnet();
            case "Buf03":
                return new Eyepatch();
            case "Buf04":
                return new RingOfStrength();
            case "Buf05":
                return new PiggyBank();
            case "Buf08":
                return new AmuletOfStrength();
            case "Buf10":
                return new Cactus();
            case "Buf11":
                return new StrengthPoison();
            case "Buf13":
                return new HealingFlask();
            case "Buf14":
                return new Antidote();
            case "Buf16":
                return new WoodenBracelet();
            case "Buf17":
                return new PoisonGrenade();
            case "Buf18":
                return new CreditCard();
            case "Buf19":
                return new VitaminPill();
            case "Buf21":
                return new EnergyDrink();
            case "Buf23":
                return new HoneyBall();
            case "Buf24":
                return new HandMirror();

            //Effect Item do Enemy Drop

            case "ei01":
                return new PosionousSpore();

            case "ei04":
                return new ThornFruit();

            case "coin":
                return new Coin();

            case "chestCommon":
                return new ChestCommon();

            case "chestRare":
                return new ChestRare();

            case "chestEpic":
                return new ChestEpic();

            default:
                Debug.LogWarning($"Không tìm thấy hành động cho ID: {id}");
                return null;
        }
    }
}