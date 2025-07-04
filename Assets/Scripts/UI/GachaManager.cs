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
    private readonly int startCoin = 10000;
    private int currentCoin = 200;
    public int CurrentCoin => currentCoin;

    private void OnEnable()
    {
        currentCoin = PlayerPrefs.GetInt("Num of Coin", startCoin);
        numOfCoinTxt.SetText(currentCoin.ToString());
    }

    public bool CanSpin()
    {
        return currentCoin >= numCoinPerSpin;
    }

    public void IncreaseCoin(int val)
    {
        int oldCoin = currentCoin;
        currentCoin += val;
        AnimateCoinText(oldCoin, currentCoin);
    }

    public void CoinAfterSpin()
    {
        int oldCoin = currentCoin;
        currentCoin -= numCoinPerSpin;
        AnimateCoinText(oldCoin, currentCoin);
    }

    public void SaveCoin()
    {
        PlayerPrefs.SetInt("Num of Coin", currentCoin);
    }

    private void AnimateCoinText(int fromValue, int toValue)
    {
        DOTween.Kill(numOfCoinTxt);

        DOVirtual.Int(fromValue, toValue, 0.5f, value =>
        {
            numOfCoinTxt.SetText(value.ToString());
        }).SetTarget(numOfCoinTxt).SetEase(Ease.OutQuad);
    }

    private void OnDrawGizmosSelected()
    {
        coinPerSpinTxt.SetText(numCoinPerSpin.ToString());
    }
}
