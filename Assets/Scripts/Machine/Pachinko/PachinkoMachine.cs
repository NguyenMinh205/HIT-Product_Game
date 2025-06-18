using Gameplay;
using System.Collections;
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
    [SerializeField] private Button dropButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private Sprite defaultItemSprite;

    private PachinkoState _state = PachinkoState.Waiting;
    private PachinkoItem _item;
    private bool _gameEnded;
    private PachinkoClaw curClaw;

    public PachinkoBox Box => box;
    public PachinkoState State => _state;
    public PachinkoItem Item => _item;
    public Transform LeftClawLimit => leftClawLimit;
    public Transform RightClawLimit => rightClawLimit;

    private void Start()
    {
        curClaw = Instantiate(claw, spawnPos.position, Quaternion.identity, this.transform.parent).GetComponent<PachinkoClaw>();
        curClaw.Init(this, spawnPos.position);
        startButton?.onClick.AddListener(StartGame);
        rollButton?.onClick.AddListener(RollItem);
    }

    private void StartGame()
    {
        if (_state != PachinkoState.Waiting) return;
        _state = PachinkoState.Movingclaw;
        _item = Instantiate(itemPrefab, curClaw.ItemPosition.position, Quaternion.identity, this.transform.parent);
        _item.Init(defaultItemSprite);
        _item.SetDrop();
    }

    public void DropItem()
    {
        if (_state != PachinkoState.Movingclaw || _item == null) return;
        _state = PachinkoState.Dropping;
    }

    public void RollItem()
    {

    }    

    public void EndGame(bool success)
    {
        if (_state != PachinkoState.Dropping) return;
        if (success)
        {
            _state = PachinkoState.Ended;
            _gameEnded = true;
            Debug.Log("Thắng: Vật phẩm trúng rổ!");
        }
        else
        {
            _state = PachinkoState.Waiting;
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