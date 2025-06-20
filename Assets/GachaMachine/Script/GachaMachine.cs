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
    }

    public void PullGacha()
    {
        if (state != GachaState.Start) return;
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
        Debug.Log("Phần thưởng nhân vật đang được triển khai...");
    }

    private void RewardSkin()
    {
        Debug.Log("Phần thưởng skin đang được triển khai...");
    }

    private void RewardCoins(int count)
    {
        Debug.Log($"Phần thưởng {count} Coin đang được triển khai...");
    }

    private void RewardNothing()
    {
        Debug.Log("Không có phần thưởng đặc biệt...");
    }
}