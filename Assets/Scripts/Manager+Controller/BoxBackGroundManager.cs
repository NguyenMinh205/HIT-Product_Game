using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxBackGroundManager : Singleton<BoxBackGroundManager>
{
    [Header("Box")]
    [SerializeField] private Sprite backGroundFightNormal;
    [SerializeField] private Sprite backGroundFightBoss;
    [SerializeField] private Sprite backGroundHealingRoom;
    [SerializeField] private Sprite backGroundMysteryRoom;

    [Space]
    [Header("Move")]
    [SerializeField] private Sprite backGroundMoveFightNormal;
    [SerializeField] private Sprite backGroundMoveFightBoss;
    [SerializeField] private Sprite backGroundMoveHealingRoom;
    [SerializeField] private Sprite backGroundMoveMysteryRoom;

    [Space]
    [Header("Basket")]
    [SerializeField] private Sprite backGroundBasketFightNormal;
    [SerializeField] private Sprite backGroundBasketFightBoss;
    [SerializeField] private Sprite backGroundBasketHealingRoom;
    [SerializeField] private Sprite backGroundBasketMysteryRoom;

    [Space]
    [Header("Perk")]
    [SerializeField] private Sprite perkFightNormal;
    [SerializeField] private Sprite perkFightBoss;
    [SerializeField] private Sprite perkHealing;
    [SerializeField] private Sprite perkMystery;

    [Space]
    [Header("Machine")]
    [SerializeField] private SpriteRenderer box;
    [SerializeField] private Image move;
    [SerializeField] private Image perkList;
    [SerializeField] private SpriteRenderer basket;

    public void SetBossRoom()
    {
        box.sprite = backGroundFightBoss;
        move.sprite = backGroundMoveFightBoss;
        basket.sprite = backGroundBasketFightBoss;
        perkList.sprite = perkFightBoss;
    }

    public void SetFightRoom()
    {
        box.sprite = backGroundFightNormal;
        move.sprite = backGroundMoveFightNormal;
        basket.sprite = backGroundBasketFightNormal;
        perkList.sprite = perkFightNormal;
    }

    public void SetHealingRoom()
    {
        box.sprite = backGroundHealingRoom;
        move.sprite = backGroundMoveHealingRoom;
        basket.sprite = backGroundBasketHealingRoom;
        perkList.sprite = perkHealing;
    }
    public void SetMysteryRoom()
    {
        box.sprite = backGroundFightBoss;
        move.sprite = backGroundMoveMysteryRoom;
        basket.sprite = backGroundBasketMysteryRoom;
        perkList.sprite = perkMystery;
    }
}
