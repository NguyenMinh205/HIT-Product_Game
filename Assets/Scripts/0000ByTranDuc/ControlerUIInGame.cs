using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TranDuc
{
    public class ControlerUIInGame : Singleton<ControlerUIInGame>
    {
        [Space]
        [Header("UI")]
        [SerializeField] private GameObject uiInRoom;
        [SerializeField] private GameObject uiMap;
        [SerializeField] private GameObject uiPachinkoRoom;
        [SerializeField] private GameObject uiTumblerRoom;
        [SerializeField] private GameObject uiSmithRoom;
        [SerializeField] private GameObject uiShredderRoom;
        [SerializeField] private GameObject rewardUI;
        [SerializeField] private GameObject finishUI;
        [SerializeField] private TextMeshProUGUI numOfCoinTxt;
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private Button btnRoll;

        public GameObject RewardUI => rewardUI;
        public GameObject FinishUI => finishUI;
        public GameObject BtnRoll => btnRoll.gameObject;
        private void Start()
        {
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                fadeCanvasGroup.gameObject.SetActive(true);
            }
        }
        public void CloseAllUI()
        {
            uiInRoom.SetActive(false);
            uiPachinkoRoom.SetActive(false);
            uiTumblerRoom.SetActive(false);
            uiSmithRoom.SetActive(false);
            uiShredderRoom.SetActive(false);
        }
        public void OpenRoom()
        {
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.gameObject.SetActive(true);
                fadeCanvasGroup.alpha = 1f;

                uiMap.SetActive(false);
                uiInRoom.SetActive(true);

                if (fadeCanvasGroup != null && Camera.main != null)
                {
                    Camera.main.orthographicSize = 8f;

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(fadeCanvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad));
                    sequence.Join(DOVirtual.Float(8f, 6f, 0.5f, value =>
                    {
                        Camera.main.orthographicSize = value;
                    }).SetEase(Ease.InOutQuad));
                    sequence.OnComplete(() =>
                    {
                        fadeCanvasGroup.gameObject.SetActive(false);
                    });
                }
            }
        }
        public void OpenRoomType(int type)
        {
            switch (type)
            {
                case 0:
                    uiTumblerRoom.SetActive(true);
                    break;
                case 1:
                    uiPachinkoRoom.SetActive(true);
                    break;
                case 2:
                    uiSmithRoom.SetActive(true);
                    break;
                case 3:
                    uiShredderRoom.SetActive(true);
                    break;
            }
        }
        public void OutRoom()
        {
            uiInRoom.SetActive(false);
            uiMap.SetActive(true);
            numOfCoinTxt.text = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin.ToString();
        }
    }
}
