using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDBackGroundBoxMachine
{
    FightNormal,
    FightBoss,
    Healing,
    Mystery,
}
public class DefaultRoomBackGround : MonoBehaviour
{
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;
    [SerializeField] private Sprite backGroundHealingRoom;
    [SerializeField] private Sprite backGroundMysteryRoom;

    private void Awake()
    {
        ObserverManager<IDBackGroundBoxMachine>.AddDesgisterEvent(IDBackGroundBoxMachine.FightNormal, SetBackGroundFightNormal);
        ObserverManager<IDBackGroundBoxMachine>.AddDesgisterEvent(IDBackGroundBoxMachine.FightBoss, SetBackGroundFightBoss);
        ObserverManager<IDBackGroundBoxMachine>.AddDesgisterEvent(IDBackGroundBoxMachine.Healing, SetBackGroundHealingRoom);
        ObserverManager<IDBackGroundBoxMachine>.AddDesgisterEvent(IDBackGroundBoxMachine.Mystery, SetBackGroundMysteryRoom);
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
