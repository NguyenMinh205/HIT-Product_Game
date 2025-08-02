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
    Sequence moveSeq;
    private RectTransform startPos;
    private RectTransform endPos;
    private bool isMove = false;
    private bool isBusy = false;
    private bool isUse = false;
    private float speed = 300f;

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
    private void OnDestroy()
    {
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        if (moveSeq != null && moveSeq.IsActive())
            moveSeq.Kill();
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

        Debug.Log("Add Move Item" + newTarget.anchoredPosition);
        isMove = true;

        float distance = Vector2.Distance(rect.anchoredPosition, newTarget.anchoredPosition);
        float duration = distance / speed;

        if (moveSeq != null && moveSeq.IsActive())
            moveSeq.Kill();

        moveSeq = DOTween.Sequence().SetAutoKill(false);

        moveSeq.Append(rect.DOAnchorPos(newTarget.anchoredPosition, duration)
                     .SetEase(Ease.Linear)
                     .OnComplete(() =>
                     {
                         OnMoveComplete();
                     }));
    }
    public void SetItemDisplay(ItemBase item)
    {
        idItem = item.id;
        icon.sprite = item.icon;
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
            Debug.Log("Check Consume Point");
            startPos = ItemTube.Instance.FallPoint;
            ItemTube.Instance.IsConsumePoint = true;
            AddMoveItem(ItemTube.Instance.ConsumePoint);
        }
    }

    public void PlayerUseItem()
    {
        if (isUse) return;

        ItemTube.Instance.UseItem(this);

        isUse = true;
        if (rotateTween != null && rotateTween.IsActive()) rotateTween.Kill();
        if(GamePlayController.Instance.EnemyController.ListEnemy.Count > 0)
            ItemTube.Instance.itemUsage.UseItem(idItem, GamePlayController.Instance.PlayerController.CurrentPlayer, GamePlayController.Instance.EnemyController.ListEnemy[0]);
        Sequence useSeq = DOTween.Sequence();
        useSeq.Join(rect.DOScale(2f, 1f).SetEase(Ease.OutBack));
        useSeq.Join(rect.DOScale(0f, 1f).SetEase(Ease.InBack));
        useSeq.Join(icon.DOFade(0f, 1f));
        useSeq.OnComplete(() => {
            ItemTube.Instance.IsConsumePoint = false;
            ItemTube.Instance.ContinueItemDisplay();
            Destroy(gameObject);
            ItemTube.Instance.CheckItemNull();
        });
    }
    public void OnMoveComplete()
    {
        isMove = false;
        Debug.Log("On Move Complete");
        switch (state)
        {
            case StateItemDisplay.Entry:
                ItemTube.Instance.IsRollPoint = true;
                state = StateItemDisplay.Roll;
                break;
            case StateItemDisplay.Roll:
                ItemTube.Instance.IsFallPoint = true;
                state = StateItemDisplay.Fall;
                break;
            case StateItemDisplay.Fall:
                ItemTube.Instance.IsConsumePoint = true;
                state = StateItemDisplay.Consume;
                break;
        }
    }
    public void CheckDistance()
    {
        switch (state)
        {
            case StateItemDisplay.Entry:
                if(ItemTube.Instance.IsEntryPoint)
                {
                    float distence = Vector2.Distance(startPos.anchoredPosition, rect.anchoredPosition);
                    if(distence >= 65)
                        ItemTube.Instance.IsEntryPoint = false;
                }
                break;
            case StateItemDisplay.Roll:
                if (ItemTube.Instance.IsRollPoint)
                {
                    float distence = Vector2.Distance(startPos.anchoredPosition, rect.anchoredPosition);
                    if (distence >= 65)
                        ItemTube.Instance.IsRollPoint = false;
                }
                break;
            case StateItemDisplay.Fall:
                if (ItemTube.Instance.IsFallPoint)
                {
                    float distence = Vector2.Distance(startPos.anchoredPosition, rect.anchoredPosition);
                    if (distence >= 65)
                        ItemTube.Instance.IsFallPoint = false;
                }
                break;
        }
    }
    public void PauseDotween()
    {
        if (rotateTween != null && rotateTween.IsActive())
        {
            rotateTween.Pause();
        }
        if (moveSeq != null && moveSeq.IsActive())
        {
            moveSeq.Pause();
        }
    }
    public void ContinueTween()
    {
        if (rotateTween != null && rotateTween.IsActive())
        {
            rotateTween.Play();
        }

        if (moveSeq != null && moveSeq.IsActive())
        {
            moveSeq.Play();
        }
    }
}
