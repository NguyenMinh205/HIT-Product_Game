using Gameplay;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
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
    [SerializeField] private Button dropButton;
    [SerializeField] private Sprite defaultItemSprite;

    private GameState _state = GameState.Waiting;
    private PachinkoItem _item;
    private bool _gameEnded;
    private PachinkoClaw curClaw;

    public PachinkoBox Box => box;
    public GameState State => _state;

    private void Start()
    {
        curClaw = Instantiate(claw, spawnPos.position, Quaternion.identity).GetComponent<PachinkoClaw>();
        curClaw.Init(this, spawnPos.position);
        startButton?.onClick.AddListener(StartGame);
        dropButton?.onClick.AddListener(DropItem);
    }

    private void StartGame()
    {
        if (_state != GameState.Waiting) return;
        _state = GameState.Movingclaw;
        _item = Instantiate(itemPrefab, curClaw.ItemPosition.position, Quaternion.identity);
        _item.Init(defaultItemSprite);
        _item.SetDrop();
    }

    public void DropItem()
    {
        if (_state != GameState.Movingclaw || _item == null) return;
        _state = GameState.Dropping;
    }

    public void EndGame(bool success)
    {
        if (_state != GameState.Dropping) return;
        if (success)
        {
            _state = GameState.Ended;
            _gameEnded = true;
            Debug.Log("Thắng: Vật phẩm trúng rổ!");
        }
        else
        {
            _state = GameState.Waiting;
            _gameEnded = false;
            Debug.Log("Thua: Vật phẩm trúng sàn! Bắt đầu lại...");
        }
        Destroy(_item.gameObject);
        if (_item != null) _item = null;
    }

    private void OnDestroy()
    {
        if (_item) Destroy(_item.gameObject);
    }
}