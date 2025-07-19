using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDBasketBackGround
{
    FightNormal,
    FightBoss,
}
public class BasketBackGround : MonoBehaviour
{
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;

    private void Awake()
    {
        ObserverManager<IDBasketBackGround>.AddDesgisterEvent(IDBasketBackGround.FightNormal, SetBackGroundFightNormal);
        ObserverManager<IDBasketBackGround>.AddDesgisterEvent(IDBasketBackGround.FightBoss, SetBackGroundFightBoss);
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
