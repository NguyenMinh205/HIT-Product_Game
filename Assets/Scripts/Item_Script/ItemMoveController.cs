using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ItemMove
{
    AddItemToMove,
}

public class ItemMoveController : MonoBehaviour
{
    [SerializeField] private Transform posStart;
    [SerializeField] private Transform posEnd;
    [SerializeField] private ItemUsage itemUsage;
    [SerializeField] private float delayBetweenItems = 0.2f;
    [SerializeField] private float pauseAfterAnyFinish = 4f;

    private Queue<Item> itemQueue = new Queue<Item>();
    private List<Tween> activeTweens = new List<Tween>();
    private bool isRunningCoroutine = false;
    private bool isPaused = false;

    private void Awake()
    {
        ObserverManager<ItemMove>.AddDesgisterEvent(ItemMove.AddItemToMove, EnqueueItem);
    }

    public void EnqueueItem(object obj)
    {
        if (obj is Item item)
        {
            itemQueue.Enqueue(item);
            item.gameObject.SetActive(false);

            if (!isRunningCoroutine && !isPaused)
                StartCoroutine(ProcessItemQueue());
        }
    }

    private IEnumerator ProcessItemQueue()
    {
        isRunningCoroutine = true;

        while (itemQueue.Count > 0)
        {
            if (isPaused) yield return null;

            Item item = itemQueue.Dequeue();
            item.gameObject.SetActive(true);
            item.SetBalloon(true);
            MoveItem(item);

            yield return new WaitForSeconds(delayBetweenItems);
        }

        isRunningCoroutine = false;
    }

    private void MoveItem(Item item)
    {
        Vector3 start = posStart.position;
        item.transform.position = start;
        Vector3 end = posEnd.position;

        Vector3 dropPoint = start + Vector3.down * 1.39f + Vector3.left * 0.37f;
        Vector3 midCurve = dropPoint + Vector3.left * 6.4f;

        Vector3[] path = new Vector3[] { start, dropPoint, midCurve, end };

        float duration = Vector3.Distance(start, end) / 5f * 2f;

        item.transform.rotation = Quaternion.identity;

        Sequence seq = DOTween.Sequence();

        Tween pathTween = item.transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.InOutSine);

        Tween rotateTween = item.transform.DORotate(new Vector3(0, 0, 360f), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(1, LoopType.Restart);

        seq.Join(pathTween);
        seq.Join(rotateTween);

        seq.OnComplete(() =>
        {
            HandleItemArrived(item);
        });

        if (isPaused)
        {
            seq.Pause();
        }

        activeTweens.Add(seq);
    }

    private void HandleItemArrived(Item item)
    {
        EffectItem(item);
    }

    public void PauseMovement()
    {
        if (isPaused) return;
        isPaused = true;

        foreach (var seq in activeTweens)
            if (seq.IsActive()) seq.Pause();
    }

    public void ResumeMovement()
    {
        if (!isPaused) return;
        isPaused = false;

        foreach (var seq in activeTweens)
            if (seq.IsActive()) seq.Play();

        // Kiểm tra và khởi động lại coroutine nếu có item trong hàng đợi
        if (!isRunningCoroutine && itemQueue.Count > 0)
            StartCoroutine(ProcessItemQueue());
    }

    public void EffectItem(Item item)
    {
        PauseMovement();
        Player player = GamePlayController.Instance.PlayerController.CurrentPlayer;
        List<Enemy> enemyList = GamePlayController.Instance.EnemyController.ListEnemy;
        Sequence fx = DOTween.Sequence();

        fx.Join(item.transform.DOScale(item.transform.localScale * 1.7f, 1f)
                 .SetEase(Ease.OutBack));

        if (enemyList.Count > 0)
            itemUsage.UseItem(item.ID, player, enemyList[0], enemyList);
        else
            itemUsage.UseItem(item.ID, player);

        var sr = item.SR;
        fx.Join(sr.DOFade(0f, 1f));
        fx.OnComplete(() =>
        {
            Destroy(item.gameObject);
            ResumeMovement();
            ObserverManager<IDItem>.PostEven(IDItem.ItemPlayer, item);
        });
    }

    public void EndGame()
    {
        foreach (var seq in activeTweens)
        {
            seq.Kill();
        }
        while (itemQueue.Count > 0)
        {
            Item item = itemQueue.Dequeue();
            if (item != null)
                Destroy(item.gameObject);
        }
    }
}