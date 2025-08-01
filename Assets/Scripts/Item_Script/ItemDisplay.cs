using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public enum StateItemDisplay
{
    Entry,
    Roll,
    Fall,
    Consume,
}
public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private string idItem;
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform rect;
    [SerializeField] private StateItemDisplay state;

    private Tween rotateTween;
    Sequence moveSeq = DOTween.Sequence();
    private RectTransform startPos;
    private RectTransform endPos;
    private bool isMove = false;
    private bool isUse = false;
    private float speed = 250f;

    private void Update()
    {


        if (isMove)
        {
            CheckDistance();
            return;
        }

        switch(state)
        {
            case StateItemDisplay.Entry:
                CheckRoll();
                break;
            case StateItemDisplay.Roll:
                CheckFall();
                break;
            case StateItemDisplay.Fall:
                CheckConsume();
                break;
            case StateItemDisplay.Consume:
                PlayerUseItem();
                break;
        }


    }
    public void RotateItemDisplay()
    {
        if (rect == null) return;
        rotateTween = rect.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                      .SetEase(Ease.Linear)
                      .SetLoops(-1, LoopType.Incremental);
    }
    public void AddMoveItem(RectTransform newTarget)
    {
        if(rect == null || newTarget == null) return;

        Debug.Log("Add Move Item");
        isMove = true;

        float distance = Vector2.Distance(rect.anchoredPosition, newTarget.anchoredPosition);
        float duration = distance / speed;

        moveSeq.Append(rect.DOAnchorPos(newTarget.anchoredPosition, duration)
                     .SetEase(Ease.Linear)
                     .OnComplete(() =>
                     {
                         switch(state)
                         {
                             case StateItemDisplay.Entry:
                                 ItemTube.Instance.IsRollPoint = true;
                                 state = StateItemDisplay.Roll;
                                 isMove = false;
                                 break;
                             case StateItemDisplay.Roll:
                                 ItemTube.Instance.IsFallPoint = true;
                                 state = StateItemDisplay.Fall;
                                 isMove = false;
                                 break;
                             case StateItemDisplay.Fall:
                                 ItemTube.Instance.IsConsumePoint = true;
                                 state = StateItemDisplay.Consume;
                                 isMove = false;
                                 break;
                         }
                     }));
    }
    public void SetItemDisplay(Item item)
    {
        idItem = item.ID;
        icon.sprite = item.SR.sprite;
        RotateItemDisplay();
        state = StateItemDisplay.Entry;
    }
    public void CheckRoll()
    {
        if(ItemTube.Instance.IsRollPoint == false)
        {
            startPos = ItemTube.Instance.EntryPoint;
            AddMoveItem(ItemTube.Instance.RollPoint);
        }
    }

    public void CheckFall()
    {
        if (ItemTube.Instance.IsFallPoint == false)
        {
            startPos = ItemTube.Instance.RollPoint;
            AddMoveItem(ItemTube.Instance.FallPoint);
        }
    }

    public void CheckConsume()
    {
        if (ItemTube.Instance.IsConsumePoint == false)
        {
            startPos = ItemTube.Instance.FallPoint;
            ItemTube.Instance.IsConsumePoint = true;
            AddMoveItem(ItemTube.Instance.ConsumePoint);
        }
    }

    public void PlayerUseItem()
    {
        if (!isUse) return;

        isUse = true;
        rotateTween.Kill();
        ItemTube.Instance.itemUsage.UseItem(idItem, GamePlayController.Instance.PlayerController.CurrentPlayer, GamePlayController.Instance.EnemyController.ListEnemy[0]);
        Sequence seq = DOTween.Sequence();

        seq.Join(rect.DOScale(1.5f, 1f).SetEase(Ease.OutQuad));
        seq.Join(icon.DOFade(0f, 1f).SetEase(Ease.OutQuad));
        seq.OnComplete(() =>
        {
            Destroy(gameObject);
            ItemTube.Instance.IsConsumePoint = false;
        });
    }

    public void CheckDistance()
    {
        switch (state)
        {
            case StateItemDisplay.Entry:
                if(ItemTube.Instance.IsEntryPoint)
                {
                    float distence = Vector2.Distance(startPos.anchoredPosition, rect.anchoredPosition);
                    if(distence >= 60)
                        ItemTube.Instance.IsEntryPoint = false;
                }
                break;
            case StateItemDisplay.Roll:
                if (ItemTube.Instance.IsRollPoint)
                {
                    float distence = Vector2.Distance(startPos.anchoredPosition, rect.anchoredPosition);
                    if (distence >= 60)
                        ItemTube.Instance.IsRollPoint = false;
                }
                break;
            case StateItemDisplay.Fall:
                if (ItemTube.Instance.IsFallPoint)
                {
                    float distence = Vector2.Distance(startPos.anchoredPosition, rect.anchoredPosition);
                    if (distence >= 60)
                        ItemTube.Instance.IsFallPoint = false;
                }
                break;
        }
    }
}
