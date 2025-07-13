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
    private Transform playerTarget;
    [SerializeField] private List<Item> listItemMove = new List<Item>();
    [SerializeField] private List<Item> listItemMoving = new List<Item>();
    private bool isRunningCoroutine = false;
    private bool isPaused = false;

    private List<Tween> activeTweens = new List<Tween>();
    [SerializeField] private float delayBetweenItems = 0.2f;
    [SerializeField] private float pauseAfterAnyFinish = 4f;

    private bool isTemporarilyPaused = false;

    private void Awake()
    {
        ObserverManager<ItemMove>.AddDesgisterEvent(ItemMove.AddItemToMove, EnqueueItem);
    }

    public void EnqueueItem(object obj)
    {
        if (obj is Item item)
        {
            listItemMove.Add(item);
            item.gameObject.SetActive(false);

            if (!isRunningCoroutine && !isPaused)
                StartCoroutine(StartMovingItems());
        }
    }

    private IEnumerator StartMovingItems()
    {
        isRunningCoroutine = true;

        /*playerTarget = GamePlayController.Instance.PlayerController.CurrentPlayer?.transform;
        if (playerTarget == null)
        {
            isRunningCoroutine = false;
            yield break;
        }
*/
        while (listItemMove.Count > 0)
        {
            //if (isPaused) yield break;
            Debug.Log("Check Item Count");
            Item item = listItemMove[0];
            listItemMoving.Add(item);
            listItemMove.RemoveAt(0);
            Debug.Log("Set True Item");
            item.gameObject.SetActive(true);
            item.SetBalloon(true);
            MoveItem(item);

            yield return new WaitForSeconds(delayBetweenItems);
        }

        isRunningCoroutine = false;
    }

    private void MoveItem(Item item)
    {
        Debug.Log("Move Item");
        //item.gameObject.SetActive(true);
        //item.GetComponent<Collider2D>().enabled = false;
        //item.GetComponent<PolygonCollider2D>().isTrigger = true;

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
            Debug.Log("Item Complete Player");

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
        listItemMoving.Remove(item);
        var player = GamePlayController.Instance.PlayerController.CurrentPlayer;
        var enemyList = GamePlayController.Instance.EnemyController.ListEnemy;

        if (itemUsage == null || player == null || enemyList == null || enemyList.Count == 0)
        {
            Debug.LogWarning("Missing references when using item");
            return;
        }

        EffectItem(item);
        itemUsage.UseItem(item.ID, player, enemyList[0]);
        ObserverManager<IDItem>.PostEven(IDItem.ItemPlayer, item);
        

        StartCoroutine(PauseThenResume());
    }

    public void PauseMovement()
    {
        if (isPaused) return;
        isPaused = true;

        // Pause tất cả tween đang chạy
        foreach (var seq in activeTweens)
            if (seq.IsActive()) seq.Pause();
    }
    private IEnumerator PauseThenResume()
    {
        PauseMovement();
        yield return new WaitForSeconds(pauseAfterAnyFinish);
        ResumeMovement();
    }
    public void ResumeMovement()
    {
        if (!isPaused) return;
        isPaused = false;

        foreach (var seq in activeTweens)
            if (seq.IsActive()) seq.Play();

        // Tiếp tục hàng đợi nếu còn item
        //if (!isRunningCoroutine && listItemMove.Count > 0)
            //StartCoroutine(StartMovingItems());
    }

    public void EffectItem(Item item)
    {
        Sequence fx = DOTween.Sequence();

        fx.Join(item.transform.DOScale(item.transform.localScale * 1.7f, 1f)
                 .SetEase(Ease.OutBack));

        var sr = item.SR;
        fx.Join(sr.DOFade(0f, 1f));
        fx.OnComplete(() =>
        {
            Destroy(item.gameObject);
            //PoolingManager.Despawn(item.gameObject);
        });
    }

    public void EndGame()
    {
        foreach (var seq in activeTweens)
        {
            seq.Kill();
        }
        foreach(Item item in listItemMoving)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
    }
}
