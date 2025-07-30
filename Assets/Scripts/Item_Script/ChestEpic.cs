using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestEpic : IItemAction
{
    public void Execute(Player player, Enemy target)
    {
        ObserverManager<IDMysteryRoom>.PostEven(IDMysteryRoom.AddChest, Rarity.Epic);
    }

    public void Upgrade()
    {
        //throw new System.NotImplementedException();
    }


}
