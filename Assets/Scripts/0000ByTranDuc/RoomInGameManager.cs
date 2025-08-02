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
           
            ControlerUIInGame.Instance.CloseAllUI();

            defaultClawMachineBox.gameObject.SetActive(false);
            pachinkoMachineBox.SetActive(false);
            tumblerMachineBox.SetActive(false);
        }

        private void OpenRoom()
        {
            CloseAllRoomsAndUIs();
            AudioManager.Instance.PlayMusicInGame();
            MapSystem.Instance.SetActiveRoomVisual(false);
            DataManager.Instance.GameData.SetKeepPlayState(true);
            GamePlayController.Instance.PlayerController.NumOfCoinInRoom.text = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin.ToString();
        }

        public void OpenRoomFight()
        {
            OpenRoom();
            defaultRoom.SetActive(true);
            defaultClawMachineBox.gameObject.SetActive(true);
            defaultClawMachineBox.SetBackground(RoomType.FightNormal);
            currentRoom = defaultRoom;
            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "FightRoom");
            GamePlayController.Instance.StartFightRoom("FightRoom");
        }

        public void OpenRoomBossFight()
        {
            OpenRoom();
            bossRoom.SetActive(true);
            defaultClawMachineBox.gameObject.SetActive(true);
            defaultClawMachineBox.SetBackground(RoomType.FightBoss);
            currentRoom = bossRoom;
            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "BossRoom");
            GamePlayController.Instance.StartFightRoom("BossRoom");
        }

        public void OpenRoomHealing()
        {
            OpenRoom();
            healingRoom.SetActive(true);
            defaultClawMachineBox.gameObject.SetActive(true);
            defaultClawMachineBox.SetBackground(RoomType.Healing);
            currentRoom = healingRoom;
            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);
            GamePlayController.Instance.StartFunctionRoom("HealingRoom");
        }

        public void OpenRoomMystery()
        {
            OpenRoom();
            mysteryRoom.SetActive(true);
            defaultClawMachineBox.gameObject.SetActive(true);
            defaultClawMachineBox.SetBackground(RoomType.Mystery);
            currentRoom = mysteryRoom;
            GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
            GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);
            GamePlayController.Instance.StartFunctionRoom("MysteryRoom");
        }

        public void OpenRoomPerkReward()
        {
            OpenRoom();
            tumblerMachineBox.SetActive(true);
            ControlerUIInGame.Instance.OpenRoomType(0);
            perkRewardRoom.SetActive(true);
            TumblerMachine.Instance.Init();
            currentRoom = perkRewardRoom;
            Debug.Log(currentRoom.name);
        }

        public void OpenRoomPachinko()
        {
            OpenRoom();
            pachinkoRoom.SetActive(true);
            ControlerUIInGame.Instance.OpenRoomType(1);
            pachinkoMachineBox.SetActive(true);
            currentRoom = pachinkoRoom;
        }

        public void OpenRoomSmith()
        {
            OpenRoom();
            smithRoom.SetActive(true);
            ControlerUIInGame.Instance.OpenRoomType(2);
            currentRoom = smithRoom;
            currentRoom.GetComponent<SmithRoomManager>().Init();
        }

        public void OpenRoomShredder()
        {
            OpenRoom();
            shredderRoom.SetActive(true);
            ControlerUIInGame.Instance.OpenRoomType(3);
            currentRoom = shredderRoom;
            currentRoom.GetComponent<ShredderRoomManager>().Init();
        }

        public void OutRoom()
        {
            if (currentRoom != null)
            {
                ControlerUIInGame.Instance.OutRoom();
                currentRoom.SetActive(false);
                CloseAllRoomsAndUIs();
                MapSystem.Instance.SetActiveRoomVisual(true);
             
                if (intoRoomTrigger != null)
                {
                    intoRoomTrigger.gameObject.SetActive(false);
                }
                currentRoom = null;
                AudioManager.Instance.PlayMusicSelectRoom();
                MapSystem.Instance.SetRoomVisited();
                ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar, GamePlayController.Instance.PlayerController.CurrentPlayer);
            }
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
            }
            else
            {
                AudioManager.Instance.PlaySoundClickButton();
                DataManager.Instance.GameData.Coin += DataManager.Instance.GameData.Player.stats.Coin;
                DataManager.Instance.GameData.ClearGameplayData();
            }
            PoolingManager.ClearAll();
            SceneManager.LoadScene(0);
        }
    }

}