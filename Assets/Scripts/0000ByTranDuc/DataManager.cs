using System.Collections.Generic;
using UnityEngine;

namespace TranDuc
{
    public class DataManager : Singleton<DataManager>
    {
        public SecureGameData GameData { get; private set; }

        protected override void Awake()
        {
            base.KeepActive(true);
            base.Awake();
            GameData = new SecureGameData();
            GameData.Load();
        }

        private void OnApplicationQuit()
        {
            GameData.Save();
            Debug.LogWarning("OK");
        }
    }

    [System.Serializable]
    public class SecureGameData
    {
        // START DATA
        [SerializeField] private float musicVolume = 0.5f;
        [SerializeField] private float soundVolume = 0.5f;
        [SerializeField] private int coin = 100;
        [SerializeField] private string selectedCharacterId = "";
        [SerializeField] private int selectedSkinIndex = 0;
        [SerializeField] private int selectedDifficultyIndex = 0;
        [SerializeField] private float extraDamagePercent = 0;
        [SerializeField] private float extraHealthPercent = 0;
        [SerializeField] private List<CharacterState> characterStates = new();
        [SerializeField] private bool isKeepingPlayGame = false;

        // MAIN GAME DATA
        [SerializeField] private PlayerData playerData = new();
        [SerializeField] private Vector2Int playerNodePosition;
        [SerializeField] private int curFloor = 0;
        [SerializeField] private MapData curMapData;
        [SerializeField] private List<Vector2Int> visitedTilePositions = new();
        [SerializeField] private List<string> usedBossIDs = new();

        #region PROPERTIES - START
        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = Mathf.Clamp01(value);
        }

        public float SoundVolume
        {
            get => soundVolume;
            set => soundVolume = Mathf.Clamp01(value);
        }

        public int Coin
        {
            get => coin;
            set => coin = Mathf.Max(0, value);
        }

        public string SelectedCharacterId
        {
            get => selectedCharacterId;
            set => selectedCharacterId = value;
        }

        public int SelectedSkinIndex
        {
            get => selectedSkinIndex;
            set => selectedSkinIndex = Mathf.Max(0, value);
        }

        public int SelectedDifficultyIndex
        {
            get => selectedDifficultyIndex;
            set => selectedDifficultyIndex = Mathf.Clamp(value, 0, 2);
        }

        public float ExtraDamagePercent => extraDamagePercent;
        public float ExtraHealthPercent => extraHealthPercent;
        public IReadOnlyList<CharacterState> CharacterStates => characterStates.AsReadOnly();
        public bool IsKeepingPlayGame => isKeepingPlayGame;
        public void SetCharacterStates(List<CharacterState> states)
        {
            if (states == null || states.Count == 0) return;
            characterStates = new List<CharacterState>(states);
        }

        public void SetExtraStats(float dmgPercent, float hpPercent)
        {
            extraDamagePercent = Mathf.Clamp(dmgPercent, 0f, 1f);
            extraHealthPercent = Mathf.Clamp(hpPercent, 0f, 1f);
        }

        public void SetKeepPlayState(bool isKeep) =>
            isKeepingPlayGame = isKeep;
        #endregion

        #region PROPERTIES - MAIN
        public PlayerData Player => playerData;
        public Vector2Int PlayerNodePosition
        {
            get => playerNodePosition;
            set => playerNodePosition = value;
        }

        public int CurrentFloor
        {
            get => curFloor;
            set => curFloor = Mathf.Max(0, value);
        }

        public MapData CurrentMapData
        {
            get => curMapData;
            set => curMapData = value;
        }

        public List<Vector2Int> VisitedTilePositions
        {
            get => visitedTilePositions;
            set => visitedTilePositions = value ?? new List<Vector2Int>();
        }
        public List<string> UsedBossIDs => usedBossIDs;
        #endregion

        #region SAVE/LOAD

        private const eData SaveFileKey = eData.SecureGameData;

        public void Save()
        {
            var saveUtil = new SaveUtility<SecureGameData>();
            saveUtil.SaveData(SaveFileKey, this);
        }

        public void Load()
        {
            var saveUtil = new SaveUtility<SecureGameData>();
            SecureGameData loadedData = new SecureGameData();
            saveUtil.LoadData(SaveFileKey, ref loadedData);
            CopyFrom(loadedData); 
        }
        private void CopyFrom(SecureGameData other)
        {
            musicVolume = other.musicVolume;
            soundVolume = other.soundVolume;
            coin = other.coin;
            selectedCharacterId = other.selectedCharacterId;
            selectedSkinIndex = other.selectedSkinIndex;
            selectedDifficultyIndex = other.selectedDifficultyIndex;
            extraDamagePercent = other.extraDamagePercent;
            extraHealthPercent = other.extraHealthPercent;
            characterStates = new List<CharacterState>(other.characterStates);
            isKeepingPlayGame = other.isKeepingPlayGame;
            if (isKeepingPlayGame)
            {
                playerData = other.playerData;
                playerNodePosition = other.playerNodePosition;
                curFloor = other.curFloor;
                curMapData = other.curMapData;
                visitedTilePositions = new List<Vector2Int>(other.visitedTilePositions);
                usedBossIDs = new List<string>(other.usedBossIDs);
            }
            else
            {
                ResetGameplayData();
            }
           
        }


        public void ClearGameplayData()
        {
            ResetGameplayData();
        }
      /*  public void ClearAllData()
        {
            SaveGameManager.DeleteSave(SaveFileKey);
            Reset();
        }*/

        private void Reset()
        {
            musicVolume = 0.5f;
            soundVolume = 0.5f;
            coin = 100;
            selectedCharacterId = "";
            selectedSkinIndex = 0;
            selectedDifficultyIndex = 0;
            extraDamagePercent = 0f;
            extraHealthPercent = 0f;
            characterStates = new List<CharacterState>();
            isKeepingPlayGame = false;

            ResetGameplayData();
        }

        private void ResetGameplayData()
        {
            playerData = new PlayerData();
            playerNodePosition = Vector2Int.zero;
            curFloor = 0;
            curMapData = null;
            visitedTilePositions = new List<Vector2Int>();
            usedBossIDs = new List<string>();
        }

        #endregion
    }

    [System.Serializable]
    public class CharacterState
    {
        public string id;
        public bool isUnlocked;
        public List<bool> skinUnlocks = new();
    }

    [System.Serializable]
    public class PlayerData
    {
        public CharacterStat stats = new();
        public Inventory inventory = new();
        public List<StartRoundBuffInfo> startRoundBuffs = new();
    }
}
