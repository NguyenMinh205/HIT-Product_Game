using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRare : IItemAction
{
    public void Execute(Player player, Enemy target)
    {
        ObserverManager<IDMysteryRoom>.PostEven(IDMysteryRoom.AddChest, Rarity.Rare);
    }

    public void Upgrade()
    {
        //throw new System.NotImplementedException();
    }

}
