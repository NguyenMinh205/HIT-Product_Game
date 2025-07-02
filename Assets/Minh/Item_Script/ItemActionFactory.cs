using UnityEngine;

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
            case "Atk15":
                return new PoisonDagger();
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
            case "Def11":
                return new MetalShield();
            case "Buf04":
                return new RingOfStrength();
            case "Buf08":
                return new AmuletOfStrength();
            case "Buf11":
                return new StrengthPoison();
            default:
                Debug.LogWarning($"Không tìm thấy hành động cho ID: {id}");
                return null;
        }
    }
}