using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TranDuc;

public class GachaManager : Singleton<GachaManager>
{
    [SerializeField] private TextMeshProUGUI numOfCoinTxt;
    [SerializeField] private TextMeshProUGUI coinPerSpinTxt;
    [SerializeField] private int numCoinPerSpin = 25;
    public int NumCoinPerSpin => numCoinPerSpin;

    private void OnEnable()
    {
        numOfCoinTxt.SetText(DataManager.Instance.GameData.Coin.ToString());
    }

    public bool CanSpin()
    {
        return DataManager.Instance.GameData.Coin >= numCoinPerSpin;
    }

    public void IncreaseCoin(int val)
    {
        int oldCoin = DataManager.Instance.GameData.Coin;
        DataManager.Instance.GameData.Coin += val;
        DataManager.Instance.GameData.Save();
        AnimateCoinText(oldCoin, DataManager.Instance.GameData.Coin);
    }

    public void CoinAfterSpin()
    {
        int oldCoin = DataManager.Instance.GameData.Coin;
        DataManager.Instance.GameData.Coin -= numCoinPerSpin;
        DataManager.Instance.GameData.Save();
        AnimateCoinText(oldCoin, DataManager.Instance.GameData.Coin);
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