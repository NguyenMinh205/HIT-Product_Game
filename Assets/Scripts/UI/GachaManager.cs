using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GachaManager : Singleton<GachaManager>
{
    [SerializeField] private TextMeshProUGUI numOfCoinTxt;
    [SerializeField] private TextMeshProUGUI coinPerSpinTxt;
    [SerializeField] private int numCoinPerSpin = 25;
    public int NumCoinPerSpin => numCoinPerSpin;

    private void OnEnable()
    {
        numOfCoinTxt.SetText(GameData.Instance.startData.coin.ToString());
    }

    public bool CanSpin()
    {
        return GameData.Instance.startData.coin >= numCoinPerSpin;
    }

    public void IncreaseCoin(int val)
    {
        int oldCoin = GameData.Instance.startData.coin;
        GameData.Instance.startData.coin += val;
        GameData.Instance.SaveStartGameData();
        AnimateCoinText(oldCoin, GameData.Instance.startData.coin);
    }

    public void CoinAfterSpin()
    {
        int oldCoin = GameData.Instance.startData.coin;
        GameData.Instance.startData.coin -= numCoinPerSpin;
        GameData.Instance.SaveStartGameData();
        AnimateCoinText(oldCoin, GameData.Instance.startData.coin);
    }

    private void AnimateCoinText(int fromValue, int toValue)
    {
        DOTween.Kill(numOfCoinTxt);

        DOVirtual.Int(fromValue, toValue, 0.5f, value =>
        {
            numOfCoinTxt.SetText(value.ToString());
        }).SetTarget(numOfCoinTxt).SetEase(Ease.OutQuad);
    }
}