using Gameplay;
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

        // Khởi tạo vật phẩm ngẫu nhiên khi bắt đầu room
        _item = Instantiate(itemPrefab, curClaw.ItemPosition.position, Quaternion.identity, this.transform.parent);
        ItemBase newItem = itemDatabase.GetRandomItem();
        while (newItem == _lastRolledItem && itemDatabase.GetItems().Count > 1) // Tránh trùng vật phẩm trước
        {
            newItem = itemDatabase.GetRandomItem();
        }
        _lastRolledItem = newItem;
        _item.Init(newItem.icon);
        _item.SetDrop();
        UpdateCoinTexts(); // Cập nhật văn bản ban đầu
    }

    private void UpdateCoinTexts()
    {
        if (coinToStartTxt != null)
            coinToStartTxt.text = coinToStart.ToString();
        if (coinToRollTxt != null)
            coinToRollTxt.text = coinToRoll.ToString();
    }

    private void StartGame()
    {
        if (_state != PachinkoState.Waiting || _item == null) return;
        if (GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin < coinToStart)
        {
            Debug.Log("Không đủ xu để bắt đầu!");
            return;
        }
        GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(-coinToStart); // Trừ xu
        _state = PachinkoState.Movingclaw;
        UpdateCoinTexts();
    }

    public void RollItem()
    {
        if (_state != PachinkoState.Waiting || _item == null) return;
        if (GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin < coinToRoll)
        {
            Debug.Log("Không đủ xu để roll!");
            return;
        }
        GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ChangeCoin(-coinToRoll);

        ItemBase newItem = itemDatabase.GetRandomItem();
        while (newItem == _lastRolledItem && itemDatabase.GetItems().Count > 1)
        {
            newItem = itemDatabase.GetRandomItem();
        }
        _lastRolledItem = newItem;
        _item.Init(newItem.icon);
        coinToRoll *= 2;
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
        if (success)
        {
            _state = PachinkoState.Ended;
            _gameEnded = true;
            GamePlayController.Instance.PlayerController.TotalInventory.AddItem(_lastRolledItem, 1);
            Debug.Log("Thắng: Vật phẩm trúng rổ!");
            GameManager.Instance.OutRoom();
        }
        else
        {
            _state = PachinkoState.Waiting;
            _gameEnded = false;
            _item = Instantiate(itemPrefab, curClaw.ItemPosition.position, Quaternion.identity, this.transform.parent);
            _item.Init(_lastRolledItem.icon);
            _item.SetDrop();
        }
        if (success)
        {
            Destroy(_item.gameObject);
            if (_item != null) _item = null;
            _lastRolledItem = null; // Reset vật phẩm trước khi kết thúc
        }
    }

    private void OnDisable()
    {
        if (_item) Destroy(_item.gameObject);
    }
}