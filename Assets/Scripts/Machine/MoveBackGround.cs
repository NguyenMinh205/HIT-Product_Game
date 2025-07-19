using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDMoveBackGround
{
    FightNormal,
    FightBoss,
}
public class MoveBackGround : MonoBehaviour
{
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;

    private void Awake()
    {
        ObserverManager<IDMoveBackGround>.AddDesgisterEvent(IDMoveBackGround.FightNormal, SetBackGroundFightNormal);
        ObserverManager<IDMoveBackGround>.AddDesgisterEvent(IDMoveBackGround.FightBoss, SetBackGroundFightBoss);
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
