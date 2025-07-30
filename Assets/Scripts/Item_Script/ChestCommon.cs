using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestCommon : IItemAction
{
    public void Execute(Player player, Enemy target)
    {
        ObserverManager<IDMysteryRoom>.PostEven(IDMysteryRoom.AddChest, Rarity.Common);
    }

    public void Upgrade()
    {
        //throw new System.NotImplementedException();
    }

}
