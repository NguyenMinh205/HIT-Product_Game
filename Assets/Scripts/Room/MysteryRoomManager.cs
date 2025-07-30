using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDMysteryRoom
{
    AddChest,
    CallReward,
}
public class MysteryRoomManager : MonoBehaviour
{
    [SerializeField] private List<Rarity> listChest;

    private void Awake()
    {
        ObserverManager<IDMysteryRoom>.AddDesgisterEvent(IDMysteryRoom.AddChest, AddChest);
        ObserverManager<IDMysteryRoom>.AddDesgisterEvent(IDMysteryRoom.CallReward, CallReward);
    }
    private void OnEnable()
    {
        listChest = new List<Rarity>();
    }

    public void AddChest(object obj)
    {
        if (obj is Rarity rarity)
        {
            if (listChest.Count < 3)
            {
                listChest.Add(rarity);
            }
        }
    }

    public void CallReward(object obj)
    {
        if(listChest.Count == 0)
        {
            Debug.LogWarning("No chests available to reward.");
            GameManager.Instance.OutRoom();
            return;
        }
        GameManager.Instance.RewardUI.SetActive(true);
        GameManager.Instance.BtnRoll.SetActive(false);
        RewardManager.Instance.InitReward(listChest);
    }
}
