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
    private Transform playerTarget;
    private Queue<Item> itemQueue = new Queue<Item>();
    private bool isRunningCoroutine = false;
    private bool isPaused = false;

    private List<Tween> activeTweens = new List<Tween>();
    [SerializeField] private float delayBetweenItems = 0.2f;
    [SerializeField] private float pauseAfterAnyFinish = 1f;

    private bool isTemporarilyPaused = false;

    private void Awake()
    {
        ObserverManager<ItemMove>.AddDesgisterEvent(ItemMove.AddItemToMove, EnqueueItem);
    }

    public void EnqueueItem(object obj)
    {
        if (obj is Item item)
        {
            itemQueue.Enqueue(item);

            if (!isRunningCoroutine && !isPaused)
                StartCoroutine(StartMovingItems());
        }
    }

    private IEnumerator StartMovingItems()
    {
        isRunningCoroutine = true;

        playerTarget = GamePlayController.Instance.playerController.CurrentPlayer?.transform;
        if (playerTarget == null)
        {
            isRunningCoroutine = false;
            yield break;
        }

        while (itemQueue.Count > 0)
        {
            if (isPaused) yield break;

            Item item = itemQueue.Dequeue();
            MoveItem(item);

            yield return new WaitForSeconds(delayBetweenItems);
        }

        isRunningCoroutine = false;
    }

    private void MoveItem(Item item)
    {
        //item.GetComponent<Collider2D>().enabled = false;
        item.GetComponent<PolygonCollider2D>().isTrigger = true;

        Vector3 start = posStart.position;
        item.transform.position = start;
        Vector3 end = playerTarget.position;
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
            Debug.Log("Item Complete Player");  /// Item Cham vao Player
            //if (!isTemporarilyPaused) StartCoroutine(PauseAllTweensTemporarily());
        });

        if (isPaused)
        {
            seq.Pause();
        }

        activeTweens.Add(seq);
    }

    private IEnumerator PauseAllTweensTemporarily()
    {
        isTemporarilyPaused = true;

        foreach (var tween in activeTweens)
        {
            if (tween.IsActive() && tween.IsPlaying())
            {
                Debug.Log("Pause Tween");
                tween.Pause();
            }
        }

        yield return new WaitForSeconds(pauseAfterAnyFinish);

        foreach (var tween in activeTweens)
        {
            if (tween.IsActive() && !tween.IsPlaying())
                tween.Play();
        }

        isTemporarilyPaused = false;
    }

/*    public void PauseMovement()
    {
        isPaused = true;

        foreach (var tween in activeTweens)
        {
            if (tween.IsActive()) tween.Pause();
        }
    }

    public void ResumeMovement()
    {
        isPaused = false;

        foreach (var tween in activeTweens)
        {
            if (tween.IsActive()) tween.Play();
        }

        if (!isRunningCoroutine && itemQueue.Count > 0)
        {
            StartCoroutine(StartMovingItems());
        }
    }*/
}
