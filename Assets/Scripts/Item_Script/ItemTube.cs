using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemTube : Singleton<ItemTube>
{
    [SerializeField] public ItemUsage itemUsage;
    [SerializeField] private List<Item> itemIDs;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject foreGround;
    [SerializeField] private RectTransform list;
    private bool isItemNull = false;
    public bool IsItemNull => isItemNull;

    [Space]
    [Header("ItemDisplay")]
    [SerializeField] private ItemDisplay itemDisplay;

    [Space]
    [Header("Movement Point")]
    [SerializeField] private RectTransform enTryPoint;
    [SerializeField] private RectTransform rollPoint;
    [SerializeField] private RectTransform fallPoint;
    [SerializeField] private RectTransform consumePoint;
    public RectTransform EntryPoint => enTryPoint;
    public RectTransform RollPoint => rollPoint;
    public RectTransform FallPoint => fallPoint;
    public RectTransform ConsumePoint => consumePoint;

    private bool isEntryPoint = false;
    private bool isRollPoint = false;
    private bool isFallPoint = false;
    private bool isConsumePoint = false;

    public bool IsEntryPoint
    {
        get => isEntryPoint;
        set
        {
            isEntryPoint = value;
            if(!value) // Entry Point rong
            {
                SpawnItemDisplay();
            }
        }
    }
    public bool IsRollPoint
    {
        get => isRollPoint;
        set
        {
            isRollPoint = value;
            if (!value) 
            {
                
            }
        }
    }
    public bool IsFallPoint
    {
        get => isFallPoint;
        set
        {
            isFallPoint = value;
            if (!value) // Fall Point rong
            {
               
            }
        }
    }
    public bool IsConsumePoint
    {
        get => isConsumePoint;
        set
        {
            isConsumePoint = value;
            if (!value) // Consume Point rong
            {

            }
        }
    }

    public void AddItem(Item item)
    {
        if (item == null) return;
        itemIDs.Add(item);
        SpawnItemDisplay();
    }
    public void SetActionBG(bool val)
    {
        if (backGround == null || foreGround == null) return;

        backGround.SetActive(val);
        foreGround.SetActive(val);
    }
    public void SpawnItemDisplay()
    {
        if (itemIDs.Count <= 0) return;
        if (isEntryPoint) return;

        ItemDisplay newItemDisplay = Instantiate(itemDisplay, enTryPoint.position, Quaternion.identity, list);
        newItemDisplay.SetItemDisplay(itemIDs[0]);
        itemIDs.RemoveAt(0);
        isEntryPoint = true;
    }

    public void CheckItemNull()
    {
        if (itemIDs.Count <= 0) isItemNull = true;
        else isItemNull = false;
        GamePlayController.Instance.CheckTurnPlayer();
    }
}
