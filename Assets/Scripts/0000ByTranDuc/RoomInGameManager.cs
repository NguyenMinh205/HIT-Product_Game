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

        private void CloseAllRoomsAndUIs()
        {
            healingRoom.SetActive(false);
            mysteryRoom.SetActive(false);
            pachinkoRoom.SetActive(false);
            smithRoom.SetActive(false);
            shredderRoom.SetActive(false);
            bossRoom.SetActive(false);
            defaultRoom.SetActive(false);
           
            ControllerUIInGame.Instance.CloseAllUI();

            defaultClawMachineBox.gameObject.SetActive(false);
            pachinkoMachineBox.SetActive(false);
            tumblerMachineBox.SetActive(false);
        }

        private void OpenRoom(string typeRoom = null)
        {
            MapSystem.Instance.CurPlayerMap.IsIntoRoom = true;

            MapSystem.Instance.SetActiveMapStore(false);

            if (currentRoom != null) currentRoom.SetActive(true);
            if (currentMachine != null) currentMachine.SetActive(true);
            ControllerUIInGame.Instance.OpenUIRoomType(typeRoom);

            Debug.LogError("OK1");
            if (currentMachine == defaultClawMachineBox.gameObject)
            {
                UiPerksList.Instance.SetActivePerk(true);
            }

            ControllerUIInGame.Instance.OpenRoom();
            Debug.LogError("OK2");
            DOVirtual.DelayedCall(0.25f, () =>
            {
                Debug.LogError("Open Room: " + typeRoom);
                CheckTypeRoom(typeRoom);
            });
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
                    PachinkoMachine.Instance.Init();
                    break;
                case "Smith":
                    currentRoom.GetComponent<SmithRoomManager>().Init();
                    break;
                case "Shredder":
                    currentRoom.GetComponent<ShredderRoomManager>().Init();
                    break;

                default:
                    Debug.Log("Unknown Room Type: " + typeRoom);
                    break;
            }
        }
        public void OpenRoomFight()
        {
            currentRoom = defaultRoom;
            currentMachine = defaultClawMachineBox.gameObject;

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "Fight");
            OpenRoom("FightRoom");
            if (currentMachine != null) defaultClawMachineBox.SetBackground(RoomType.FightNormal);
        }

        public void OpenRoomBossFight()
        {
            currentRoom = bossRoom;
            currentMachine = defaultClawMachineBox.gameObject;

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "BossRoom");
            OpenRoom("BossRoom");
            if (currentMachine != null) defaultClawMachineBox.SetBackground(RoomType.FightBoss);
        }

        public void OpenRoomHealing()
        {
            currentRoom = healingRoom;
            currentMachine = defaultClawMachineBox.gameObject;

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);

            OpenRoom("HealingRoom");
            if (currentMachine != null) defaultClawMachineBox.SetBackground(RoomType.Healing);
        }

        public void OpenRoomMystery()
        {
            currentRoom = mysteryRoom;
            currentMachine = defaultClawMachineBox.gameObject;

            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);

            OpenRoom("MysteryRoom");
            if (currentMachine != null) defaultClawMachineBox.SetBackground(RoomType.Mystery);
        }

        public void OpenRoomPerkReward()
        {
            currentRoom = perkRewardRoom;
            currentMachine = tumblerMachineBox;

            OpenRoom("PerkReward");
        }

        public void OpenRoomPachinko()
        {
            currentRoom = pachinkoRoom;
            currentMachine = pachinkoMachineBox;

            OpenRoom("Pachinko");
        }

        public void OpenRoomSmith()
        {
            currentRoom = smithRoom;

            OpenRoom("Smith");
        }

        public void OpenRoomShredder()
        {
            currentRoom = shredderRoom;

            OpenRoom("Shredder");
        }

        public void OutRoom()
        {
            if (currentRoom != null)
            {
                UiPerksList.Instance.SetActivePerk(false);

                if (currentRoom != null) currentRoom.SetActive(false);
                if (currentMachine != null) currentMachine.SetActive(false);

                MapSystem.Instance.SetActiveMapStore(true);

                MapSystem.Instance.CurPlayerMap.IsIntoRoom = false;
                MapSystem.Instance.CurPlayerMap.IsMoving = false;

                if (intoRoomTrigger != null)
                {
                    Debug.LogError("Out Room: " + intoRoomTrigger.IdNameRoom);
                    Vector3 posNew = intoRoomTrigger.gameObject.transform.position;
                    intoRoomTrigger.gameObject.SetActive(false);
                }

                AudioManager.Instance.PlayMusicSelectRoom();
                MapSystem.Instance.SetRoomWhenWin();
                ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar, GamePlayController.Instance.PlayerController.CurrentPlayer);
            }
            currentRoom = null;
            currentMachine = null;
            ControllerUIInGame.Instance.OutRoom();
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
            SettingInGame.Instance.BackHome();
            PoolingManager.ClearAll();
            SceneManager.LoadScene(0);
        }
    }
}