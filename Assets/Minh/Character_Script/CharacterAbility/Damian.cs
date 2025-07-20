using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damian : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {

    }

    public void StartSetupStat()
    {
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeMaxHP(10);
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCurHP(10);
        GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeBloodsuckingRate(0.15f);
    }
}
