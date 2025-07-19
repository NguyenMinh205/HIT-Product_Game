using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDBackGroundBoxMachine
{
    FightNormal,
    FightBoss,
}
public class DefaultRoomBackGround : MonoBehaviour
{
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;

    private void Awake()
    {
        ObserverManager<IDBackGroundBoxMachine>.AddDesgisterEvent(IDBackGroundBoxMachine.FightNormal, SetBackGroundFightNormal);
        ObserverManager<IDBackGroundBoxMachine>.AddDesgisterEvent(IDBackGroundBoxMachine.FightBoss, SetBackGroundFightBoss);
    }
    public void SetBackGroundFightNormal(object obj = null)
    {
        GetComponent<SpriteRenderer>().sprite = backGroundFightNormal;
    }

    public void SetBackGroundFightBoss(object obj = null)
    {
        GetComponent<SpriteRenderer>().sprite = backGroundFightBoss;
    }
}
