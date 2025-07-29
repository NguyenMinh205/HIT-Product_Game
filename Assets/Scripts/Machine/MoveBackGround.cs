using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDMoveBackGround
{
    FightNormal,
    FightBoss,
    Healing,
    Mystery,
}
public class MoveBackGround : MonoBehaviour
{
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;
    [SerializeField] private Sprite backGroundHealingRoom;
    [SerializeField] private Sprite backGroundMysteryRoom;

    private void Awake()
    {
        ObserverManager<IDMoveBackGround>.AddDesgisterEvent(IDMoveBackGround.FightNormal, SetBackGroundFightNormal);
        ObserverManager<IDMoveBackGround>.AddDesgisterEvent(IDMoveBackGround.FightBoss, SetBackGroundFightBoss);
        ObserverManager<IDMoveBackGround>.AddDesgisterEvent(IDMoveBackGround.Healing, SetBackGroundHealingRoom);
        ObserverManager<IDMoveBackGround>.AddDesgisterEvent(IDMoveBackGround.Mystery, SetBackGroundMysteryRoom);
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
