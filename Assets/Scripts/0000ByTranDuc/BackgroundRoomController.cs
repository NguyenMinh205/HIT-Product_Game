using UnityEngine;

public enum RoomType
{
    FightNormal,
    FightBoss,
    Healing,
    Mystery
}

public class BackgroundRoomController : MonoBehaviour
{
    [Header("SpriteRenderers")]
    [SerializeField] private SpriteRenderer srDefaultRoomBackGround;
    [SerializeField] private SpriteRenderer srBasketBackGround;
    [SerializeField] private SpriteRenderer srMoveBackGround;

    [Header("Sprites")]
    [SerializeField] private Sprite[] fightNormalSprites;
    [SerializeField] private Sprite[] fightBossSprites;
    [SerializeField] private Sprite[] healingRoomSprites;
    [SerializeField] private Sprite[] mysteryRoomSprites;


    public void SetBackground(RoomType type)
    {
        Sprite[] selectedSprites = null;

        switch (type)
        {
            case RoomType.FightNormal:
                selectedSprites = fightNormalSprites;
                break;
            case RoomType.FightBoss:
                selectedSprites = fightBossSprites;
                break;
            case RoomType.Healing:
                selectedSprites = healingRoomSprites;
                break;
            case RoomType.Mystery:
                selectedSprites = mysteryRoomSprites;
                break;
        }

        if (selectedSprites == null || selectedSprites.Length < 3)
        {
            Debug.LogWarning("Sprites không hợp lệ hoặc chưa đủ 3 sprite.");
            return;
        }

        srDefaultRoomBackGround.sprite = selectedSprites[0];
        srBasketBackGround.sprite = selectedSprites[1];
        srMoveBackGround.sprite = selectedSprites[2];
    }
}
