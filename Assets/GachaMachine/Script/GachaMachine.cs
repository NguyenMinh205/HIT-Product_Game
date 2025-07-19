using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GachaState
{
    Start,
    Spinning,
    End
}

public class GachaMachine : Singleton<GachaMachine>
{
    public List<GachaItem> possibleItems;
    public GameObject slot1, slot2, slot3;
    public float spinDuration = 2f;
    public float spinSpeed = 0.1f;
    [SerializeField] private CharacterDatabaseSO characterDatabaseSO;

    private GachaState state;
    private Image sr1, sr2, sr3;
    private GachaItem item1, item2, item3;

    public GachaState State => state;

    void Start()
    {
        state = GachaState.Start;
        sr1 = slot1.GetComponent<Image>();
        sr2 = slot2.GetComponent<Image>();
        sr3 = slot3.GetComponent<Image>();
        Debug.Log("GachaMachine State: " + state);
        if (sr1 == null || sr2 == null || sr3 == null) Debug.LogError("One or more slots not assigned with Image component!");
        if (characterDatabaseSO == null) Debug.LogError("CharacterDatabaseSO not assigned!");
        else characterDatabaseSO.LoadUnlockedStates(); // Tải trạng thái mở khóa từ PlayerPrefs
    }

    public void PullGacha()
    {
        if (state != GachaState.Start) return;
        if (!GachaManager.Instance.CanSpin())
        {
            Debug.LogWarning("Not enough coins to spin!");
            return;
        }
        StartCoroutine(SpinSlots());
    }

    private IEnumerator SpinSlots()
    {
        state = GachaState.Spinning;
        int randomChance = Random.Range(0, 100);
        DetermineReward(randomChance);

        StartCoroutine(SpinSlot(sr1, item1, 1f));
        StartCoroutine(SpinSlot(sr2, item2, 1.5f));
        StartCoroutine(SpinSlot(sr3, item3, 2f));
        yield return new WaitForSeconds(0.5f);

        ApplyReward();
        Restart();
    }

    private void DetermineReward(int chance)
    {
        List<GachaItem> items = new List<GachaItem>();
        GachaItem character = possibleItems.Find(item => item.itemName == "Character");
        GachaItem skin = possibleItems.Find(item => item.itemName == "Skin");
        GachaItem coin = possibleItems.Find(item => item.itemName == "Coin");

        if (chance < 25)
        {
            item1 = character;
            item2 = character;
            item3 = character;
        }
        else if (chance >= 25 && chance < 50)
        {
            item1 = skin;
            item2 = skin;
            item3 = skin;
        }
        else if (chance >= 50 && chance < 60)
        {
            item1 = coin;
            item2 = coin;
            item3 = coin;
        }
        else if (chance >= 60 && chance < 75)
        {
            items.Add(coin);
            items.Add(coin);
            items.Add(Random.value < 0.5f ? character : skin);
            Shuffle(items);
            item1 = items[0];
            item2 = items[1];
            item3 = items[2];
        }
        else if (chance >= 75 && chance < 90)
        {
            items.Add(coin);
            items.Add(Random.value < 0.5f ? character : skin);
            items.Add(Random.value < 0.5f ? character : skin);
            Shuffle(items);
            item1 = items[0];
            item2 = items[1];
            item3 = items[2];
        }
        else if (chance >= 90 && chance < 100)
        {
            items.Add(character);
            items.Add(skin);
            items.Add(Random.value < 0.5f ? character : skin);
            Shuffle(items);
            item1 = items[0];
            item2 = items[1];
            item3 = items[2];
        }
    }

    private void Shuffle(List<GachaItem> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GachaItem temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private IEnumerator SpinSlot(Image sr, GachaItem finalItem, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            sr.sprite = possibleItems[Random.Range(0, possibleItems.Count)].icon;
            timer += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }
        sr.sprite = finalItem.icon;
    }

    private void Restart()
    {
        state = GachaState.Start;
    }

    private void ApplyReward()
    {
        int characterCount = (item1.itemName == "Character" ? 1 : 0) + (item2.itemName == "Character" ? 1 : 0) + (item3.itemName == "Character" ? 1 : 0);
        int skinCount = (item1.itemName == "Skin" ? 1 : 0) + (item2.itemName == "Skin" ? 1 : 0) + (item3.itemName == "Skin" ? 1 : 0);
        int coinCount = (item1.itemName == "Coin" ? 1 : 0) + (item2.itemName == "Coin" ? 1 : 0) + (item3.itemName == "Coin" ? 1 : 0);

        if (characterCount == 3)
        {
            Debug.Log("3 Character! Gọi RewardCharacter()");
            RewardCharacter();
        }
        else if (skinCount == 3)
        {
            Debug.Log("3 Skin! Gọi RewardSkin()");
            RewardSkin();
        }
        else if (coinCount == 3)
        {
            Debug.Log("3 Coin! Gọi RewardCoins(3)");
            RewardCoins(3);
        }
        else if (coinCount == 2)
        {
            Debug.Log("2 Coin! Gọi RewardCoins(2)");
            RewardCoins(2);
        }
        else if (coinCount == 1)
        {
            Debug.Log("1 Coin! Gọi RewardCoins(1)");
            RewardCoins(1);
        }
        else
        {
            Debug.Log("Nothing! Gọi RewardNothing()");
            RewardNothing();
        }
    }

    private void RewardCharacter()
    {
        AudioManager.Instance.PlayRewardSound();
        if (characterDatabaseSO == null || characterDatabaseSO.characters.Count == 0)
        {
            Debug.LogWarning("CharacterDatabaseSO is null or empty! Cannot reward character.");
            return;
        }

        // Tìm danh sách nhân vật chưa mở khóa
        List<Character> lockedCharacters = characterDatabaseSO.characters.FindAll(c => !c.isUnlocked);
        if (lockedCharacters.Count == 0)
        {
            Debug.LogWarning("No locked characters available to unlock!");
            return;
        }

        // Chọn ngẫu nhiên một nhân vật chưa mở khóa
        Character characterToUnlock = lockedCharacters[Random.Range(0, lockedCharacters.Count)];
        characterDatabaseSO.UnlockCharacter(characterToUnlock.id);
    }

    private void RewardSkin()
    {
        AudioManager.Instance.PlayRewardSound();
        if (characterDatabaseSO == null || characterDatabaseSO.characters.Count == 0)
        {
            Debug.LogWarning("CharacterDatabaseSO is null or empty! Cannot reward skin.");
            return;
        }

        // Chọn ngẫu nhiên một nhân vật
        Character randomCharacter = characterDatabaseSO.characters[Random.Range(0, characterDatabaseSO.characters.Count)];
        if (randomCharacter.skins == null || randomCharacter.skins.Count == 0)
        {
            Debug.LogWarning($"Character {randomCharacter.name} has no skins!");
            return;
        }

        // Tìm danh sách skin chưa mở khóa của nhân vật
        List<int> lockedSkinIndices = new List<int>();
        for (int i = 0; i < randomCharacter.skins.Count; i++)
        {
            if (!randomCharacter.skins[i].isUnlocked)
            {
                lockedSkinIndices.Add(i);
            }
        }

        if (lockedSkinIndices.Count == 0)
        {
            Debug.LogWarning($"No locked skins available for character {randomCharacter.name}!");
            return;
        }

        // Chọn ngẫu nhiên một skin chưa mở khóa
        int skinIndexToUnlock = lockedSkinIndices[Random.Range(1, lockedSkinIndices.Count)];
        characterDatabaseSO.UnlockSkin(randomCharacter.id, skinIndexToUnlock);
    }

    private void RewardCoins(int count)
    {
        AudioManager.Instance.PlayCoin();
        float multiplier;
        switch (count)
        {
            case 3:
                multiplier = Random.Range(2f, 3f); // x2 đến x3
                break;
            case 2:
                multiplier = Random.Range(1f, 2f); // x1 đến x2
                break;
            case 1:
                multiplier = Random.Range(0.25f, 0.75f); // x0.25 đến x0.75
                break;
            default:
                multiplier = 0f;
                break;
        }

        int rewardCoins = Mathf.CeilToInt(multiplier * GachaManager.Instance.NumCoinPerSpin);
        GachaManager.Instance.IncreaseCoin(rewardCoins);
        Debug.Log($"Rewarded {rewardCoins} coins (multiplier: {multiplier}x)");
    }

    private void RewardNothing()
    {
        Debug.Log("No special reward given.");
    }
}