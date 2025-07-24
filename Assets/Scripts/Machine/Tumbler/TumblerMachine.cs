 using Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum TumblerState
{
    Spawning,
    Waiting,
    Tumblering,
    Selecting,
    Ended
}

public class TumblerMachine : Singleton<TumblerMachine>
{
    [SerializeField] private TumblerItem itemPrefabs;
    [SerializeField] private Transform perkStore;
    [SerializeField] private List<PerkBase> perkList;
    [SerializeField] private TumblerBox tumblerBox;
    [SerializeField] private Transform perkMachineTumbler;
    [SerializeField] private Button startButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private int numItemsInTumbler = 8;
    [SerializeField] private int maxItemsToCollect = 2;
    public int MaxItemsToCollect => maxItemsToCollect;
    [SerializeField] private float maxRotationSpeed = 100f;
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float decelerationTime = 1f;

    private TumblerState _state = TumblerState.Spawning;
    private bool _finished;
    private float _currentRotation;
    private float _currentRotationSpeed;
    private List<TumblerItem> _spawnedItems;
    private List<TumblerItem> _droppedItems;
    private TumblerItem _selectedItem;

    public TumblerBox TumblerBox => tumblerBox;
    public List<TumblerItem> DroppedItems => _droppedItems;
    public TumblerState State => _state;

    private void Start()
    {
        startButton?.onClick.AddListener(StartTumbler);
        leaveButton?.onClick.AddListener(LeaveGame);
        if (tumblerBox == null || perkMachineTumbler == null)
        {
            Debug.LogError("TumblerBox hoặc PerkMachineTumbler chưa được gán!");
        }
    }

    public void Init()
    {
        perkMachineTumbler.rotation = Quaternion.Euler(0, 0, 0);
        _spawnedItems = new List<TumblerItem>();
        _droppedItems = new List<TumblerItem>();
        StartCoroutine(SpawnInitialItems());
    }

    private IEnumerator SpawnInitialItems()
    {
        if (itemPrefabs == null || perkList.Count == 0) yield return null;
        List<PerkBase> availableItems = new List<PerkBase>(perkList);
        for (int i = 0; i < numItemsInTumbler && availableItems.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            TumblerItem item = PoolingManager.Spawn(itemPrefabs, tumblerBox.SpawnPoint.position, Quaternion.identity, perkStore);
            item.Init(availableItems[randomIndex]);
            _spawnedItems.Add(item);
            availableItems.RemoveAt(randomIndex);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForEndOfFrame();
        tumblerBox.Collider.enabled = true;
        _state = TumblerState.Waiting;
    }

    private void StartTumbler()
    {
        if (_state != TumblerState.Waiting) return;
        _state = TumblerState.Tumblering;
        _currentRotation = 0f;
        _currentRotationSpeed = 0f;
        _droppedItems.Clear();
        StartCoroutine(TumblerLoop());
    }

    private IEnumerator TumblerLoop()
    {
        float elapsedTime = 0f;
        while (elapsedTime < accelerationTime)
        {
            elapsedTime += Time.deltaTime;
            _currentRotationSpeed = Mathf.Lerp(0f, maxRotationSpeed, elapsedTime / accelerationTime);
            _currentRotation += _currentRotationSpeed * Time.deltaTime;
            perkMachineTumbler.rotation = Quaternion.Euler(0, 0, _currentRotation);
            yield return null;
        }

        while (_droppedItems.Count < maxItemsToCollect && _state == TumblerState.Tumblering)
        {
            _currentRotation += maxRotationSpeed * Time.deltaTime;
            perkMachineTumbler.rotation = Quaternion.Euler(0, 0, _currentRotation);
            yield return null;
        }

        // Giảm tốc và dừng
        elapsedTime = 0f;
        float initialSpeed = maxRotationSpeed;
        while (elapsedTime < decelerationTime)
        {
            elapsedTime += Time.deltaTime;
            _currentRotationSpeed = Mathf.Lerp(initialSpeed, 0f, elapsedTime / decelerationTime);
            if (_currentRotationSpeed < 0.1f) _currentRotationSpeed = 0f;
            _currentRotation += _currentRotationSpeed * Time.deltaTime;
            perkMachineTumbler.rotation = Quaternion.Euler(0, 0, _currentRotation);
            yield return null;
        }

        _state = TumblerState.Selecting;
        foreach (TumblerItem item in _spawnedItems)
        {
            if (item != null) PoolingManager.Despawn(item.gameObject);
        }
        _spawnedItems.Clear();
        ShowSelectItemUI();
    }

    public void OnItemCollected(TumblerItem item)
    {
        if (_state == TumblerState.Tumblering && _spawnedItems.Contains(item))
        {
            _droppedItems.Add(item);
            _spawnedItems.Remove(item);
        }
    }

    public void ShowSelectItemUI()
    {
        GameManager.Instance.RewardUI.SetActive(true);
        RewardManager.Instance.InitReward();
    }

    public void SelectItem(TumblerItem item)
    {
        if (_state == TumblerState.Selecting && _selectedItem == null)
        {
            _selectedItem = item;
            Debug.Log($"Đã chọn vật phẩm: {_selectedItem.name}");
        }
    }

    private void LeaveGame()
    {
        if (_state == TumblerState.Selecting)
        {
            _finished = true;
            CleanUp();
        }
    }

    private void CleanUp()
    {
        foreach (var item in _spawnedItems)
        {
            if (item != null) Destroy(item.gameObject);
        }
        foreach (var item in _droppedItems)
        {
            if (item != null) Destroy(item.gameObject);
        }
        _spawnedItems.Clear();
        _droppedItems.Clear();
        _selectedItem = null;
        _finished = false;
        _state = TumblerState.Waiting;
        _currentRotation = 0f;
        perkMachineTumbler.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        if (_state == TumblerState.Tumblering)
        {
            _currentRotation += _currentRotationSpeed * Time.deltaTime;
            perkMachineTumbler.rotation = Quaternion.Euler(0, 0, _currentRotation);
        }
    }
}