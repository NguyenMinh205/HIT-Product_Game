using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class RewardManager : Singleton<RewardManager>
{
    [SerializeField] private GameObject rewardUI;
    [SerializeField] private RewardDetailUI rewardDetailPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private ItemDatabase itemDatabase;
    [Space]
    [Header("Button")]
    [SerializeField] private Button takeBtn;
    [SerializeField] private Button rollBtn;
    [SerializeField] private TextMeshProUGUI coinTxt;
    private int coinToRoll = 2;
    private int numOfReward = 3;
    private int rerollFreeTurn = 0;
    private RewardDetailUI selectedReward;
    private int commonRate = 65;
    public int CommonRate { get { return commonRate; } set { commonRate = value; } }
    private int rareRate = 25;
    public int RareRate { get { return rareRate; } set { rareRate = value; } }
    private List<ItemBase> lastRolledItems = new List<ItemBase>();
    private List<PerkBase> perkRewards = new List<PerkBase>();

    private void Start()
    {
        takeBtn.onClick.AddListener(TakeReward);
        rollBtn.onClick.AddListener(RollReward);
        takeBtn.interactable = false;
    }

    public void InitReward()
    {
        foreach (Transform child in content)
        {
            PoolingManager.Despawn(child.gameObject);
        }

        if (GameManager.Instance.CurrentRoom == GameManager.Instance.PerkRewardRoom)
        {
            perkRewards.Clear();
            foreach (TumblerItem item in TumblerMachine.Instance.DroppedItems)
            {
                perkRewards.Add(item.PerkBase);
            }    
            foreach (PerkBase perk in perkRewards)
            {
                RewardDetailUI reward = PoolingManager.Spawn(rewardDetailPrefab, this.transform.position, Quaternion.identity, content);
                reward.Init(perk);
            }
            rollBtn.gameObject.SetActive(false);
            return;
        }
        List<ItemBase> selectedItems = new List<ItemBase>();

        for (int i = 0; i < numOfReward; i++)
        {
            ItemBase randomItem = null;
            int attempts = 0;
            const int maxAttempts = 10;

            while (attempts < maxAttempts)
            {
                Rarity rarity = GetRandomRarity();
                randomItem = itemDatabase.GetRandomItem(rarity);

                if (randomItem != null &&
                    !selectedItems.Contains(randomItem) &&
                    !lastRolledItems.Contains(randomItem))
                {
                    selectedItems.Add(randomItem);
                    break;
                }
                attempts++;
            }

            if (randomItem != null)
            {
                RewardDetailUI reward = PoolingManager.Spawn(rewardDetailPrefab, this.transform.position, Quaternion.identity, content);
                reward.Init(randomItem);
            }
        }

        rerollFreeTurn = GamePlayController.Instance.PlayerController.CurPlayerStat.RerollFreeTurn;
        lastRolledItems.Clear();
        lastRolledItems.AddRange(selectedItems);
        UpdateUI();
    }

    private Rarity GetRandomRarity()
    {
        int rarityRoll = UnityEngine.Random.Range(0, 100);
        if (rarityRoll < commonRate) return Rarity.Common;
        else if (rarityRoll < commonRate + rareRate) return Rarity.Rare;
        else return Rarity.Epic;
    }

    public void TakeReward()
    {
        if (selectedReward != null)
        {
            if (GameManager.Instance.CurrentRoom == GameManager.Instance.PerkRewardRoom)
            {
                selectedReward.Perk.Execute();
                ObserverManager<IDPerkUI>.PostEven(IDPerkUI.AddPerk, selectedReward.Perk);
                rollBtn.gameObject.SetActive(true);
                rewardUI.SetActive(false);
                GameManager.Instance.OutRoom();
                return;
            }
            GamePlayController.Instance.PlayerController.TotalInventory.AddItem(selectedReward.Item.id, 1);
            foreach (Transform child in content)
            {
                PoolingManager.Despawn(child.gameObject);
            }
            coinToRoll = 2;
            selectedReward = null;
            rewardUI.SetActive(false);
            lastRolledItems.Clear();
            GameManager.Instance.OutRoom();
        }
    }

    public void RollReward()
    {
        int reduceCoin = (int)Math.Floor(coinToRoll * GamePlayController.Instance.PlayerController.CurPlayerStat.PriceReduction);
        if (rerollFreeTurn > 0)
        {
            rerollFreeTurn--;
            InitReward();
            UpdateUI();
        }
        else if (GamePlayController.Instance.PlayerController.CurPlayerStat.Coin >= (coinToRoll - reduceCoin))
        {
            GamePlayController.Instance.PlayerController.CurPlayerStat.ChangeCoin(-(coinToRoll - reduceCoin));
            coinToRoll *= 2;
            InitReward();
            UpdateUI();
        }
        else
        {
            Debug.Log("Không đủ coin để roll!");
        }
    }

    public void SelectReward(RewardDetailUI reward)
    {
        if (selectedReward != null)
        {
            selectedReward.transform.localScale = Vector3.one;
            selectedReward.MarkChoice.color = reward.ColorDefault;
        }

        selectedReward = reward;
        selectedReward.transform.DOScale(1.1f, 0.2f).OnComplete(() => selectedReward.transform.DOScale(1f, 0.2f));
        selectedReward.MarkChoice.color = Color.yellow;
        takeBtn.interactable = true;
    }

    private void UpdateUI()
    {
        if (rerollFreeTurn > 0)
        {
            coinTxt.text = "0";
        }
        coinTxt.text = coinToRoll.ToString();
        coinTxt.color = GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin >= coinToRoll ? Color.white : Color.red;
    }
}