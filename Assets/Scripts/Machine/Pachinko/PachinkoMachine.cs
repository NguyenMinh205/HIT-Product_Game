using DG.Tweening;
using Gameplay;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PachinkoState
{
    Waiting,
    Movingclaw,
    Dropping,
    Ended
}

public class PachinkoMachine : Singleton<PachinkoMachine>
{
    [SerializeField] private PachinkoItem itemPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private PachinkoClaw claw;
    [SerializeField] private PachinkoBox box;
    [SerializeField] private Transform leftClawLimit;
    [SerializeField] private Transform rightClawLimit;
    [SerializeField] private Button startButton;
    [SerializeField] private int coinToStart;
    [SerializeField] private TextMeshProUGUI coinToStartTxt;
    [SerializeField] private Button rollButton;
    [SerializeField] private int coinToRoll;
    [SerializeField] private TextMeshProUGUI coinToRollTxt;
    [SerializeField] private ItemDatabase itemDatabase;

    [Space]
    [Header("Item Detail")]
    [SerializeField] private GameObject itemDetail;
    [SerializeField] private Image itemDetailBG;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;


    private PachinkoState _state = PachinkoState.Waiting;
    private PachinkoItem _item;
    private bool _gameEnded;
    private PachinkoClaw curClaw;
    private ItemBase _lastRolledItem;

    public PachinkoBox Box => box;
    public PachinkoState State => _state;
    public PachinkoItem Item => _item;
    public Transform LeftClawLimit => leftClawLimit;
    public Transform RightClawLimit => rightClawLimit;

    private void OnEnable()
    {
        curClaw = Instantiate(claw, spawnPos.position, Quaternion.identity, this.transform.parent).GetComponent<PachinkoClaw>();
        curClaw.Init(this, spawnPos.position);
        startButton?.onClick.AddListener(StartGame);
        rollButton?.onClick.AddListener(RollItem);
        _state = PachinkoState.Waiting;

        // Khởi tạo vật phẩm ngẫu nhiên khi bắt đầu room
        _item = PoolingManager.Spawn(itemPrefab, curClaw.ItemPosition.position, Quaternion.identity, this.transform.parent);
        ItemBase newItem = itemDatabase.GetRandomItem();
        _lastRolledItem = newItem;
        _item.Init(newItem);
        _item.SetDrop();
        UpdateItemDetail();
        UpdateCoinTexts(); // Cập nhật văn bản ban đầu
    }

    public void UpdateItemDetail()
    {
        if (_lastRolledItem != null)
        {
            CanvasGroup canvasGroup = itemDetail.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = itemDetail.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;

            detailIcon.sprite = _lastRolledItem.icon;
            detailIcon.SetNativeSize();
            detailIcon.rectTransform.sizeDelta *= 0.7f;
            detailName.text = _lastRolledItem.itemName;
            detailDescription.text = _lastRolledItem.description;
            itemDetailBG.color = _lastRolledItem.itemRarity switch
            {
                Rarity.Common => commonColor,
                Rarity.Rare => rareColor,
                Rarity.Epic => epicColor,
                _ => Color.white
            };

            canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    private void UpdateCoinTexts()
    {
        if (coinToStartTxt != null)
        {
            coinToStartTxt.text = coinToStart.ToString();
            coinToStartTxt.color = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin >= coinToStart ? Color.white : Color.red;
        }
        if (coinToRollTxt != null)
        {
            coinToRollTxt.text = coinToRoll.ToString();
            coinToRollTxt.color = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin >= coinToRoll ? Color.white : Color.red;
        }
    }

    public void StartGame()
    {
        if (_state != PachinkoState.Waiting || _item == null) return;
        int reduceCoin = (int)Math.Floor(coinToStart * GamePlayController.Instance.PlayerController.CurPlayerStat.PriceReduction);
        if (GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin < (coinToStart - reduceCoin))
        {
            return;
        }
        GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(-(coinToStart - reduceCoin));
        _state = PachinkoState.Movingclaw;
        UpdateCoinTexts();
    }

    public void RollItem()
    {
        if (_state != PachinkoState.Waiting || _item == null) return;
        int reduceCoin = (int)Math.Floor(coinToRoll * GamePlayController.Instance.PlayerController.CurPlayerStat.PriceReduction);
        if (GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin < (coinToRoll - reduceCoin))
        {
            return;
        }
        GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(-(coinToRoll - reduceCoin));

        ItemBase newItem = itemDatabase.GetRandomItem();
        while (newItem == _lastRolledItem && itemDatabase.GetItems().Count > 1)
        {
            newItem = itemDatabase.GetRandomItem();
        }
        _lastRolledItem = newItem;
        _item.Init(newItem);
        coinToRoll *= 2;
        UpdateItemDetail();
        UpdateCoinTexts();
    }

    public void DropItem()
    {
        if (_state != PachinkoState.Movingclaw || _item == null) return;
        _state = PachinkoState.Dropping;
    }

    public void EndGame(bool success)
    {
        if (_state != PachinkoState.Dropping) return;
        PoolingManager.Despawn(_item.gameObject);
        _item = null;
        if (success)
        {
            _state = PachinkoState.Ended;
            _gameEnded = true;
            GamePlayController.Instance.PlayerController.TotalInventory.AddItem(_lastRolledItem.id, 1);
            Debug.Log("Thắng: Vật phẩm trúng rổ!");
            coinToRoll = 2;
            coinToStart += 2;
            Destroy(curClaw.gameObject);
            GameManager.Instance.OutRoom();
        }
        else
        {
            _state = PachinkoState.Waiting;
            _gameEnded = false;
            _item = PoolingManager.Spawn(itemPrefab, curClaw.ItemPosition.position, Quaternion.identity, this.transform.parent);
            _item.Init(_lastRolledItem);
            _item.SetDrop();
        }
        _lastRolledItem = null;
    }

    private void OnDisable()
    {
        if (_item) Destroy(_item.gameObject);
        if (curClaw) Destroy(curClaw.gameObject);
    }
}