using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRare : IItemAction
{
    public void Execute(Player player, Enemy target)
    {
        GameManager.Instance.RewardUI.SetActive(true);
        RewardManager.Instance.InitReward();
    }

    public void Upgrade()
    {
        //throw new System.NotImplementedException();
    }

}
