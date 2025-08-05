using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TranDuc
{
    public class RoomInGameManager : Singleton<RoomInGameManager>
    {
        [Header("Room")]
        [SerializeField] private GameObject healingRoom;
        [SerializeField] private GameObject mysteryRoom;
        [SerializeField] private GameObject pachinkoRoom;
        [SerializeField] private GameObject smithRoom;
        public GameObject SmithRoom => smithRoom;
        [SerializeField] private GameObject shredderRoom;
        [SerializeField] private GameObject bossRoom;
        [SerializeField] private GameObject defaultRoom;
        [SerializeField] private GameObject perkRewardRoom;
        public GameObject PerkRewardRoom => perkRewardRoom;
        private IntoRoomTrigger intoRoomTrigger;
        private GameObject currentRoom;
        public GameObject CurrentRoom => currentRoom;

        [Space]
        [Header("Machine")]
        [SerializeField] private BackgroundRoomController defaultClawMachineBox;
        [SerializeField] private GameObject pachinkoMachineBox;
        [SerializeField] private GameObject tumblerMachineBox;
        private GameObject currentMachine;


        private bool isFinishGame = false;
        public bool IsFinishGame
        {
            get => isFinishGame;
            set
            {
                isFinishGame = value;
            }
        }
        public IntoRoomTrigger IntoRoom
        {
            get => intoRoomTrigger;
            set
            {
                intoRoomTrigger = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlayMusicSelectRoom();
            isFinishGame = false;
        }

        private void OpenRoom(string typeRoom = null)
        {
            /*if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.gameObject.SetActive(true);
                fadeCanvasGroup.alpha = 1f;
            }*/

            //AudioManager.Instance.PlayMusicInGame();
            /*PlayerMapController.Instance.IsIntoRoom = true;

            MapController.Instance.SetActiveMapStore(false);
            MapManager.Instance.SetActiveRoomVisual(false);

            uiMap.SetActive(false);
            uiInRoom.SetActive(true);*/

            if (currentRoom != null) currentRoom.SetActive(true); // Open Room
            if (currentMachine != null) currentMachine.SetActive(true); //Open Machine

            //if (currentUI != null) currentUI.SetActive(true);

            if (currentMachine == defaultClawMachineBox)
            {
                UiPerksList.Instance.SetActivePerk(true);
                ItemTube.Instance.SetActionBG(true);
            }

            GamePlayController.Instance.PlayerController.NumOfCoinInRoom.text = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin.ToString();

            Debug.Log("Open UI Room");
            ControlerUIInGame.Instance.OpenRoom(typeRoom);
        }
        public void CheckTypeRoom(string typeRoom)
        {
            switch (typeRoom)
            {
                case "BossRoom":
                    Debug.Log("Start Boss Room");
                    GamePlayController.Instance.StartFightRoom(typeRoom);
                    break;
                case "FightRoom":
                    Debug.Log("Start Fight Room");
                    GamePlayController.Instance.StartFightRoom(typeRoom);
                    break;
                case "HealingRoom":
                    Debug.Log("Start Healing Room");
                    GamePlayController.Instance.StartFightRoom(typeRoom);
                    break;
                case "MysteryRoom":
                    Debug.Log("Start Mystery Room");
                    GamePlayController.Instance.StartFightRoom(typeRoom);
                    break;
                case "PerkReward":
                    TumblerMachine.Instance.Init();
                    break;

                case "Pachinko":
                    break;

                case "Smith":
                    currentRoom.GetComponent<SmithRoomManager>().Init();
                    break;

                case "Shredder":
                    currentRoom.GetComponent<ShredderRoomManager>().Init();
                    break;

                default:
                    break;
            }
        }
        public void OpenRoomFight()
        {
            Debug.Log("Open Fight In Game Manager");
            currentRoom = defaultRoom;
            currentMachine = defaultClawMachineBox.gameObject;
            //currentUI = uiInRoom;

            //if (currentMachine != null) BoxBackGroundManager.Instance.SetFightRoom();

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "Fight");

            OpenRoom("FightRoom"); 
        }

        public void OpenRoomBossFight()
        {
            currentRoom = bossRoom;
            currentMachine = defaultClawMachineBox.gameObject;
            //currentUI = uiInRoom;
            if (currentMachine != null) BoxBackGroundManager.Instance.SetBossRoom();

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "BossRoom");

            OpenRoom("BossRoom");
        }

        public void OpenRoomHealing()
        {
            currentRoom = healingRoom;
            currentMachine = defaultClawMachineBox.gameObject;
            //currentUI = uiInRoom;
            if (currentMachine != null) BoxBackGroundManager.Instance.SetHealingRoom();

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);

            OpenRoom("HealingRoom");
        }

        public void OpenRoomMystery()
        {
            currentRoom = mysteryRoom;
            currentMachine = defaultClawMachineBox.gameObject;
            //currentUI = uiInRoom;
            if (currentMachine != null) BoxBackGroundManager.Instance.SetMysteryRoom();

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);

            OpenRoom("MysteryRoom");
        }

        public void OpenRoomPerkReward()
        {
            currentRoom = perkRewardRoom;
            currentMachine = tumblerMachineBox;
            //currentUI = uiTumblerRoom;

            OpenRoom("PerkReward");
            ControlerUIInGame.Instance.OpenRoomType(0);
        }

        public void OpenRoomPachinko()
        {
            currentRoom = pachinkoRoom;
            currentMachine = pachinkoMachineBox;
            //currentUI = uiPachinkoRoom;

            OpenRoom("Pachinko");
            ControlerUIInGame.Instance.OpenRoomType(1);
        }

        public void OpenRoomSmith()
        {
            currentRoom = smithRoom;
            //currentUI = uiSmithRoom;

            OpenRoom("Smith");
            ControlerUIInGame.Instance.OpenRoomType(2);
        }

        public void OpenRoomShredder()
        {
            currentRoom = shredderRoom;
            //currentUI = uiShredderRoom;

            OpenRoom("Shredder");
            ControlerUIInGame.Instance.OpenRoomType(3);
        }

        public void OutRoom()
        {
            if (currentRoom != null)
            {
                if (currentMachine == defaultClawMachineBox)
                {
                    UiPerksList.Instance.SetActivePerk(false);
                    ItemTube.Instance.SetActionBG(false);
                }

                if (currentRoom != null) currentRoom.SetActive(false);
                //if (currentUI != null) currentUI.SetActive(false);
                if (currentMachine != null) currentMachine.SetActive(false);

                /*MapController.Instance.SetActiveMapStore(true);
                MapManager.Instance.SetActiveRoomVisual(true);
                uiMap.SetActive(true);
                PlayerMapController.Instance.IsIntoRoom = false;
                PlayerMapController.Instance.IsMoving = false;*/

                if (intoRoomTrigger != null)
                {
                    Debug.LogError("Out Room: " + intoRoomTrigger.IdNameRoom);
                    Vector3 posNew = intoRoomTrigger.gameObject.transform.position;
                    //PlayerMapController.Instance.gameObject.transform.position = posNew;
                    //PlayerMapController.Instance.PosInGrid = new Vector2Int((int)posNew.x, (int)posNew.y);
                    intoRoomTrigger.gameObject.SetActive(false);
                }

                AudioManager.Instance.PlayMusicSelectRoom();
                //MapController.Instance.SetRoomVisited(PlayerMapController.Instance.PosInMap);
                ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar, GamePlayController.Instance.PlayerController.CurrentPlayer);
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    //numOfCoinTxt.text = GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin.ToString();
                });
            }
            currentRoom = null;
            //currentUI = null;
            currentMachine = null;
            ControlerUIInGame.Instance.OutRoom();
        }

        public void BackHome()
        {
            if (GamePlayController.Instance.IsLoseGame)
            {
                isFinishGame = true;
                DataManager.Instance.GameData.SetKeepPlayState(false);
                DataManager.Instance.GameData.ClearGameplayData();
                GamePlayController.Instance.IsLoseGame = false;
                SceneManager.LoadScene(0);
                return;
            }
            if (!isFinishGame)
            {
                AudioManager.Instance.PlaySoundClickButton();
                DataManager.Instance.GameData.SetKeepPlayState(true);
                DataManager.Instance.GameData.Save();
            }
            else
            {
                AudioManager.Instance.PlaySoundClickButton();
                DataManager.Instance.GameData.Coin += DataManager.Instance.GameData.Player.stats.Coin;
                DataManager.Instance.GameData.SetKeepPlayState(false);
                DataManager.Instance.GameData.ClearGameplayData();
            }
            PoolingManager.ClearAll();
            SceneManager.LoadScene(0);
        }
    }
}