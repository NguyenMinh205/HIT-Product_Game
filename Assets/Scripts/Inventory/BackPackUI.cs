using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Pack : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private GameObject listInvenItem;
    [SerializeField] private GameObject listInvenPerk;


    [Space]
    [Header("Setup")]
    [SerializeField] private List<string> listTitle;

    private int indexTitle = 0;
    private bool isTransitioning = false;


    public void CheckBtnLeft()
    {
        if(indexTitle == 0)
        {
            btnLeft.gameObject.SetActive(false);
        }
        else
        {
            btnLeft.gameObject.SetActive(true);
        }
    }
    public void CheckBtnRight()
    {
        if (indexTitle == listTitle.Count - 1)
        {
            btnRight.gameObject.SetActive(false);
        }
        else
        {
            btnRight.gameObject.SetActive(true);
        }
    }
    public void CheckButton()
    {
        CheckBtnLeft();
        CheckBtnRight();
        SetList();
    }

    public void OnLeft()
    {
        if (indexTitle > 0)
        {
            AnimateTileChange(indexTitle - 1, true);
        }
    }
    public void OnRight()
    {
        if (indexTitle < listTitle.Count - 1)
        {
            AnimateTileChange(indexTitle + 1, false);
        }
    }
    public void SetTile()
    {
        title.text = listTitle[indexTitle];
    }
    public void SetList()
    {
        if (title.text == "Items")
        {
            listInvenItem.SetActive(true);
            listInvenPerk.SetActive(false);
        }
        else if (title.text == "Perks")
        {
            listInvenItem.SetActive(false);
            listInvenPerk.SetActive(true);
        }

    }

    private void AnimateTileChange(int newIndex, bool slideLeft)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        float slideDistance = 50f; // hoặc chiều rộng tile
        float duration = 0.3f;

        Vector2 startPos = contentPanel.anchoredPosition;
        Vector2 offscreenPos = startPos + new Vector2(slideLeft ? slideDistance : -slideDistance, 0);

        // Fade out + Slide out
        Sequence seq = DOTween.Sequence();
        seq.Append(contentGroup.DOFade(0f, duration / 2));
        seq.Join(contentPanel.DOAnchorPos(offscreenPos, duration / 2));

        // Đổi nội dung sau khi ra ngoài
        seq.AppendCallback(() =>
        {
            indexTitle = newIndex;
            SetTile();
            SetList();
            CheckButton();
            // Đặt panel sang phía ngược lại
            contentPanel.anchoredPosition = startPos - new Vector2(slideLeft ? slideDistance : -slideDistance, 0);
        });

        // Fade in + Slide in
        seq.Append(contentPanel.DOAnchorPos(startPos, duration / 2));
        seq.Join(contentGroup.DOFade(1f, duration / 2));

        seq.OnComplete(() => isTransitioning = false);
    }
}
