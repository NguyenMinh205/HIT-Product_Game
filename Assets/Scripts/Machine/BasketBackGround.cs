using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDBasketBackGround
{
    FightNormal,
    FightBoss,
    Healing,
    Mystery,
}
public class BasketBackGround : MonoBehaviour
{
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;
    [SerializeField] private Sprite backGroundHealingRoom;
    [SerializeField] private Sprite backGroundMysteryRoom;

    private void Awake()
    {
        ObserverManager<IDBasketBackGround>.AddDesgisterEvent(IDBasketBackGround.FightNormal, SetBackGroundFightNormal);
        ObserverManager<IDBasketBackGround>.AddDesgisterEvent(IDBasketBackGround.FightBoss, SetBackGroundFightBoss);
        ObserverManager<IDBasketBackGround>.AddDesgisterEvent(IDBasketBackGround.Healing, SetBackGroundHealingRoom);
        ObserverManager<IDBasketBackGround>.AddDesgisterEvent(IDBasketBackGround.Mystery, SetBackGroundMysteryRoom);
    }
    public void SetBackGroundFightNormal(object obj = null)
    {
        GetComponent<SpriteRenderer>().sprite = backGroundFightNormal;
    }

    public void SetBackGroundFightBoss(object obj = null)
    {
        GetComponent<SpriteRenderer>().sprite = backGroundFightBoss;
    }
    public void SetBackGroundHealingRoom(object obj = null)
    {
        GetComponent<SpriteRenderer>().sprite = backGroundHealingRoom;
    }
    public void SetBackGroundMysteryRoom(object obj = null)
    {
        GetComponent<SpriteRenderer>().sprite = backGroundMysteryRoom;
    }
}
