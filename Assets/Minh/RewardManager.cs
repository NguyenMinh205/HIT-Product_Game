using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    private RewardDetailUI selectedReward;
    private int commonRate = 70;
    public int CommonRate { get { return commonRate; } set { commonRate = value; } }
    private int rareRate = 20;
    private int RareRate { get { return rareRate; } set { rareRate = value; } }

    private void Start()
    {
        takeBtn.onClick.AddListener(TakeReward);
        rollBtn.onClick.AddListener(RollReward);
        takeBtn.interactable = false;
        UpdateUI();
    }

    public void InitReward()
    {
        foreach (Transform child in content)
        {
            PoolingManager.Despawn(child.gameObject);
        }

        for (int i = 0; i < numOfReward; i++)
        {
            RewardDetailUI reward = PoolingManager.Spawn(rewardDetailPrefab, this.transform.position, Quaternion.identity, content);
            Rarity rarity = GetRandomRarity();
            ItemBase randomItem = itemDatabase.GetRandomItem(rarity);
            if (randomItem != null)
            {
                reward.Init(randomItem);
            }
        }
    }

    private Rarity GetRandomRarity()
    {
        int rarityRoll = Random.Range(0, 100);
        if (rarityRoll < commonRate) return Rarity.Common;
        else if (rarityRoll < rareRate) return Rarity.Rare;
        else return Rarity.Epic;
    }

    public void TakeReward()
    {
        if (selectedReward != null)
        {
            PlayerManager.Instance.TotalInventory.AddItem(selectedReward.Item, 1);
            foreach (Transform child in content)
            {
                PoolingManager.Despawn(child.gameObject);
            }
            coinToRoll = 2;
            selectedReward = null;
            takeBtn.interactable = false;
            rewardUI.SetActive(false);
        }
    }

    public void RollReward()
    {
        if (PlayerManager.Instance.CurrentPlayer.Stats.Coin >= coinToRoll)
        {
            PlayerManager.Instance.CurrentPlayer.Stats.ChangeCoin(-coinToRoll);
            PlayerManager.Instance.UpdateCoinText();
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
            selectedReward.MarkChoice.color = Color.white;
        }

        selectedReward = reward;
        selectedReward.transform.DOScale(1.1f, 0.2f).OnComplete(() => selectedReward.transform.DOScale(1f, 0.2f));
        selectedReward.MarkChoice.color = Color.yellow;
        takeBtn.interactable = true;
    }

    private void UpdateUI()
    {
        coinTxt.text = coinToRoll.ToString();
    }
}