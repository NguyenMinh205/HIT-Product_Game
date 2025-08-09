using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TranDuc
{
    public class ControllerUIInGame : Singleton<ControllerUIInGame>
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
        [SerializeField] private TextMeshProUGUI numOfCoinInRoomTxt;
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private Button btnRoll;

        public GameObject RewardUI => rewardUI;
        public GameObject FinishUI => finishUI;
        public GameObject BtnRoll => btnRoll.gameObject;

        private GameObject curUIRoom;
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
                UpdateNumOfCoinInRoom(GamePlayController.Instance.PlayerController.CurPlayerStat.Coin);
            }
        }
        public void OpenUIRoomType(string type)
        {
            switch (type)
            {
                case "PerkReward":
                    uiTumblerRoom.SetActive(true);
                    curUIRoom = uiTumblerRoom;
                    break;
                case "Pachinko":
                    uiPachinkoRoom.SetActive(true);
                    curUIRoom = uiPachinkoRoom;
                    break;
                case "Smith":
                    uiSmithRoom.SetActive(true);
                    curUIRoom = uiSmithRoom;
                    break;
                case "Shredder":
                    uiShredderRoom.SetActive(true);
                    curUIRoom = uiShredderRoom;
                    break;
                default:
                    break;
            }
        }

        public void UpdateNumOfCoinInMap(int num)
        {
            numOfCoinTxt.text = num.ToString();
        }

        public void UpdateNumOfCoinInRoom(int num)
        {
            numOfCoinInRoomTxt.text = num.ToString();
        }

        public void OutRoom()
        {
            uiInRoom.SetActive(false);
            if (curUIRoom != null)
            {
                curUIRoom.SetActive(false);
            }
            uiMap.SetActive(true);
            UpdateNumOfCoinInMap(GamePlayController.Instance.PlayerController.CurPlayerStat.Coin);
        }
    }
}
