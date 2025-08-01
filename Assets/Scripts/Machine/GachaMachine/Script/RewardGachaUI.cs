using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RewardGachaUI : MonoBehaviour
{
    public GameObject coinReward;
    public TextMeshProUGUI coinAmountText;
    public GameObject characterIcon;
    public Image characterImage;
    public Button okButton;

    private void Awake()
    {
        okButton.onClick.AddListener(Hide);
        gameObject.SetActive(false);
    }

    public void ShowCoinReward(int amount)
    {
        coinReward.SetActive(true);
        coinAmountText.text = $"+{amount}";
        characterIcon.SetActive(false);
        ShowWithZoomEffect();
    }

    public void ShowCharacterReward(Sprite sprite)
    {
        coinReward.SetActive(false);
        characterIcon.SetActive(true);
        characterImage.sprite = sprite;
        ShowWithZoomEffect();
    }

    public void ShowSkinReward(Sprite sprite)
    {
        coinReward.SetActive(false);
        characterIcon.SetActive(true);
        characterImage.sprite = sprite;
        ShowWithZoomEffect();
    }

    public void ShowNoReward()
    {
        coinReward.SetActive(false);
        characterIcon.SetActive(false);
        ShowWithZoomEffect();
    }

    private void ShowWithZoomEffect()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void Hide()
    {
        transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }
}