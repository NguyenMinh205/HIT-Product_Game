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
    private int commonRate = 65;
    public int CommonRate { get { return commonRate; } set { commonRate = value; } }
    private int rareRate = 25;
    public int RareRate { get { return rareRate; } set { rareRate = value; } }
    private List<ItemBase> lastRolledItems = new List<ItemBase>(); // Lưu 3 vật phẩm của lần roll trước

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

        lastRolledItems.Clear();
        lastRolledItems.AddRange(selectedItems);
    }

    private Rarity GetRandomRarity()
    {
        int rarityRoll = Random.Range(0, 100);
        if (rarityRoll < commonRate) return Rarity.Common;
        else if (rarityRoll < commonRate + rareRate) return Rarity.Rare;
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
            rewardUI.SetActive(false);
            lastRolledItems.Clear();
            GameManager.Instance.OutRoom();
        }
    }

    public void RollReward()
    {
        if (PlayerManager.Instance.CurrentPlayer.Stats.Coin >= coinToRoll)
        {
            PlayerManager.Instance.CurrentPlayer.Stats.ChangeCoin(-coinToRoll);
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
        coinTxt.text = coinToRoll.ToString();
    }
}