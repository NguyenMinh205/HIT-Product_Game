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
            default:
                Debug.LogWarning($"Không tìm thấy hành động cho ID: {id}");
                return null;
        }
    }
}