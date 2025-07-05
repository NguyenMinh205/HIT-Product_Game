using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemMoveController : Singleton<ItemMoveController>
{

    private Transform playerTarget; // nơi item bay đến (có thể là vùng giữ item)
    private Queue<Item> itemQueue = new Queue<Item>();
    private bool isProcessing = false;

    private bool isPaused = false;
    private Tween currentTween; //

    public void EnqueueItem(Item item)
    {
        itemQueue.Enqueue(item);
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (isProcessing || itemQueue.Count == 0 || isPaused) return;

        playerTarget = GamePlayController.Instance.playerController.CurrentPlayer?.transform;
        if (playerTarget == null) return;

        isProcessing = true;

        Item nextItem = itemQueue.Dequeue();
        nextItem.GetComponent<Collider2D>().enabled = false;

        Vector3 start = nextItem.transform.position;
        Vector3 end = playerTarget.position;
        Vector3 dropPoint = start + Vector3.down * 1.7f + Vector3.left * 0.55f;
        Vector3 midCurve = dropPoint + Vector3.left * 7.1f;
        Vector3[] path = new Vector3[] { start, dropPoint, midCurve, end };

        float duration = Vector3.Distance(start, end) / 5f * 10f;

        nextItem.transform.rotation = Quaternion.identity;

        Sequence seq = DOTween.Sequence();
        currentTween = seq;

        seq.Join(nextItem.transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.InOutSine));

        seq.Join(nextItem.transform.DORotate(new Vector3(0, 0, 360f), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(1, LoopType.Restart));

        seq.OnComplete(() => {
            currentTween = null;
            OnPlayerDone();
        });

        if (isPaused)
        {
            seq.Pause(); // Nếu pause khi tween vừa tạo
        }
    }

    // Được gọi sau khi player xử lý xong 1 item
    private void OnPlayerDone()
    {
        isProcessing = false;
        TryProcessNext();
    }

    public void PauseMovement()
    {
        isPaused = true;
        currentTween?.Pause();
    }

    public void ResumeMovement()
    {
        isPaused = false;
        currentTween?.Play();

        // Nếu không có tween nào đang chạy (pause trước khi tạo tween)
        if (!isProcessing)
        {
            TryProcessNext();
        }
    }
}
