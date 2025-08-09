using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private RewardGachaUI rewardGachaUI;

    private GachaState state;
    private Image sr1, sr2, sr3;
    private GachaItem item1, item2, item3;

    public GachaState State => state;

    private void OnEnable()
    {
        state = GachaState.Start;
        sr1 = slot1.GetComponent<Image>();
        sr2 = slot2.GetComponent<Image>();
        sr3 = slot3.GetComponent<Image>();
        Debug.Log("GachaMachine State: " + state);
        if (sr1 == null || sr2 == null || sr3 == null)
        if (characterDatabaseSO == null) Debug.LogError("CharacterDatabaseSO not assigned!");
        else characterDatabaseSO.LoadUnlockedStates();
        if (rewardGachaUI == null) Debug.LogError("RewardGachaUI not assigned!");
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
        yield return new WaitForSeconds(2.5f);
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
        }
    }

    private void RewardCharacter()
    {
        AudioManager.Instance.PlayRewardSound();
        if (characterDatabaseSO == null || characterDatabaseSO.characters.Count == 0)
        {
            Debug.LogError("CharacterDatabaseSO is null or empty! Cannot reward character.");
            return;
        }

        List<Character> lockedCharacters = characterDatabaseSO.characters.FindAll(c => !c.isUnlocked);
        if (lockedCharacters.Count == 0)
        {
            Debug.LogError("No locked characters available to unlock!");
            return;
        }

        Debug.LogError($"Found {lockedCharacters.Count} locked characters to choose from.");
        Character characterToUnlock = lockedCharacters[Random.Range(0, lockedCharacters.Count)];
        characterDatabaseSO.UnlockCharacter(characterToUnlock.id);
        Sprite characterSprite = characterToUnlock.skins[0].skin;
        rewardGachaUI.ShowCharacterReward(characterSprite);
    }

    private void RewardSkin()
    {
        AudioManager.Instance.PlayRewardSound();
        if (characterDatabaseSO == null || characterDatabaseSO.characters.Count == 0)
        {
            Debug.LogError("CharacterDatabaseSO is null or empty! Cannot reward skin.");
            return;
        }

        List<Character> availableCharacters = new List<Character>();
        foreach (var character in characterDatabaseSO.characters)
        {
            if (character.skins != null && character.skins.Count > 1)
            {
                bool hasLockedSkin = false;
                for (int i = 1; i < character.skins.Count; i++)
                {
                    if (!character.skins[i].isUnlocked)
                    {
                        hasLockedSkin = true;
                        break;
                    }
                }
                if (hasLockedSkin)
                {
                    availableCharacters.Add(character);
                }
            }
        }

        if (availableCharacters.Count == 0)
        {
            Debug.LogWarning("No characters with available skins to unlock!");
            return;
        }

        Character randomCharacter = availableCharacters[Random.Range(0, availableCharacters.Count)];

        List<int> lockedSkinIndices = new List<int>();
        for (int i = 1; i < randomCharacter.skins.Count; i++)
        {
            if (!randomCharacter.skins[i].isUnlocked)
            {
                lockedSkinIndices.Add(i);
            }
        }

        int skinIndexToUnlock = lockedSkinIndices[Random.Range(0, lockedSkinIndices.Count)];
        characterDatabaseSO.UnlockSkin(randomCharacter.id, skinIndexToUnlock);
        Sprite skinSprite = randomCharacter.skins[skinIndexToUnlock].skin;
        rewardGachaUI.ShowSkinReward(skinSprite);
    }

    private void RewardCoins(int count)
    {
        AudioManager.Instance.PlayCoin();
        float multiplier = 0;
        switch (count)
        {
            case 3:
                multiplier = Random.Range(2f, 3f);
                break;
            case 2:
                multiplier = Random.Range(1f, 2f);
                break;
            case 1:
                multiplier = Random.Range(0.25f, 0.75f);
                break;
        }

        int rewardCoins = Mathf.CeilToInt(multiplier * GachaManager.Instance.NumCoinPerSpin);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            GachaManager.Instance.IncreaseCoin(rewardCoins);
            Debug.LogError($"Rewarded {rewardCoins} coins (multiplier: {multiplier}x)");
            rewardGachaUI.ShowCoinReward(rewardCoins);
        });
    }
}