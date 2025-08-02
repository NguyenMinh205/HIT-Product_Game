
using System;
using System.Collections.Generic;
using TMPro;
using TranDuc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : Singleton<MapSystem>
{
    [SerializeField] private List<MapData> fightMaps;
    [SerializeField] private List<MapData> bossMaps;
    [SerializeField] private List<MapData> restMaps;
    [SerializeField] private TextMeshProUGUI floorTxt;
    [SerializeField] private TextMeshProUGUI floorInRoomTxt;
    [SerializeField] private int numFloor;

    public int NumFloor => numFloor;
    private int currentMapIndex = 0;
    public int MapIndex => currentMapIndex;

    [SerializeField] private GameObject RoomVisual;
    [SerializeField] private PlayerInMap playerPrefab;
    [SerializeField] private CharacterDatabaseSO characterDatabase;
    [SerializeField] private Transform mapStore;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private GameObject currentMapInstance;
    private MapRuntimeInstance currentMapInstanceData;
    private PlayerInMap newPlayer;

    protected override void Awake() => base.Awake();

    private void Start()
    {
        if (DataManager.Instance.GameData.IsKeepingPlayGame)
        {
            currentMapIndex = DataManager.Instance.GameData.CurrentFloor;
            LoadMap(DataManager.Instance.GameData.CurrentMapData);
        }
        else
        {
            currentMapIndex = 0;
            LoadInitialMap();
        }
    }

    private void UpdateFloorText()
    {
        floorTxt.SetText($"Floor {currentMapIndex}/{numFloor}");
        floorInRoomTxt.SetText($"Floor {currentMapIndex}/{numFloor}");
    }

    public void SetActiveRoomVisual(bool val)
    {
        RoomVisual.SetActive(val);
        mapStore.gameObject.SetActive(val);

        if (newPlayer == null) return;
        newPlayer.IsIntoRoom = !val;
        newPlayer.IsMoving = !val;
    }

    private void LoadInitialMap()
    {
        if (fightMaps.Count == 0) return;

        currentMapIndex++;
        var mapData = fightMaps[0];
        currentMapInstanceData = mapData.CreateRuntimeInstance();
        DataManager.Instance.GameData.CurrentFloor = currentMapIndex;
        DataManager.Instance.GameData.CurrentMapData = mapData;

        SpawnMap(mapData);
        SetupMap();
        GenerateSequenceMap();
        UpdateFloorText();
    }

    private void LoadMap(MapData mapData)
    {
        if (mapData == null)
        {
            LoadInitialMap();
            return;
        }

        currentMapInstanceData = mapData.CreateRuntimeInstance();
        SpawnMap(mapData);
        SetupMap();
        GenerateSequenceMap();
        UpdateFloorText();
    }

    private void SpawnMap(MapData mapData)
    {
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }

        currentMapInstance = PoolingManager.Spawn(mapData.MapPrefab,
            transform.position - new Vector3(offsetX, offsetY, 0), Quaternion.identity, mapStore);
    }

    private void SetupMap()
    {
        var tilemap = currentMapInstance.GetComponentInChildren<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("No Tilemap found in map prefab!");
            return;
        }

        var visited = DataManager.Instance.GameData.VisitedTilePositions;

        foreach (var tileEntry in currentMapInstanceData.tileGrid)
        {
            var tile = tileEntry.Value;
            var gridPos = tile.position;
            if (visited.Contains(gridPos))
            {
                tile.visited = true;
            }
            Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
            Vector3 adjustedPos = new Vector3(worldPos.x + offsetX, worldPos.y + offsetY / 2, worldPos.z);

/*
            if (DataManager.Instance.GameData.IsKeepingPlayGame)
            {
                if (newPlayer != null) Destroy(newPlayer);
                var posData = DataManager.Instance.GameData.PlayerNodePosition;
                Vector3 posPlayer = posData - new Vector2(gridX, gridY);
                newPlayer = PoolingManager.Spawn(playerPrefab, adjustedPos + posPlayer, Quaternion.identity, mapStore);
                newPlayer.Initialize(tilemap, currentMapData, posData, posData - new Vector2Int(bounds.xMin, bounds.yMin),
                    characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId)
                        .skins[DataManager.Instance.GameData.SelectedSkinIndex].skin);
            }
            else
            {
                if (newPlayer != null) Destroy(newPlayer);
                DataManager.Instance.GameData.PlayerNodePosition = new Vector2Int(x, y);
                newPlayer = PoolingManager.Spawn(playerPrefab, adjustedPos, Quaternion.identity, mapStore);
                newPlayer.Initialize(tilemap, currentMapData, new Vector2Int(gridX, gridY),
                    DataManager.Instance.GameData.PlayerNodePosition,
                    characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId)
                        .skins[DataManager.Instance.GameData.SelectedSkinIndex].skin);
            }*/

            var tileType = tile.tileTypes;
            switch (tileType)
            {
                case EMapTileType.Empty:
                case EMapTileType.Nothing:
                    break;
                case EMapTileType.Entrance:
                    PoolingManager.Spawn(tile.iconPrefab, adjustedPos, Quaternion.identity, mapStore);
                    /*    if (DataManager.Instance.GameData.IsKeepingPlayGame)
                 {
                     var posData = DataManager.Instance.GameData.PlayerNodePosition;
                     Vector3 posPlayer = posData + Vector2.zero;
                     newPlayer = PoolingManager.Spawn(playerPrefab, posPlayer, Quaternion.identity, mapStore);
                     newPlayer.Initialize(tilemap, currentMapInstanceData, posData, gridPos,
                         characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId)
                             .skins[DataManager.Instance.GameData.SelectedSkinIndex].skin);
                 }
                 else
                 {
                 }*/
                    if(newPlayer != null) Destroy(newPlayer);
                    newPlayer = PoolingManager.Spawn(playerPrefab, adjustedPos, Quaternion.identity, mapStore);
                    newPlayer.Initialize(tilemap, currentMapInstanceData, gridPos, gridPos,
                        characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId)
                            .skins[DataManager.Instance.GameData.SelectedSkinIndex].skin);
                    break;
                case EMapTileType.Exit:
                    GameObject exitObj = PoolingManager.Spawn(tile.iconPrefab, adjustedPos, Quaternion.identity, mapStore);
                    var trigger = exitObj.GetComponent<ExitTrigger>();
                    if (trigger != null)
                    {
                        currentMapInstanceData.AddExitTrigger(trigger);
                    }
                    break;
                default:
                    if (!tile.visited)
                    {
                        PoolingManager.Spawn(tile.iconPrefab, adjustedPos, Quaternion.identity, mapStore);
                    }
                    break;
            }
        }
    }
    public void SetRoomVisited()
    {
        Vector2Int position = newPlayer.PosInMap;
        if (currentMapInstanceData.tileGrid.TryGetValue(position, out var tile))
        {
            if (!tile.visited && tile.tileTypes == EMapTileType.Empty)
            {
                tile.visited = true;
                if (!DataManager.Instance.GameData.VisitedTilePositions.Contains(position))
                {
                    DataManager.Instance.GameData.VisitedTilePositions.Add(position);
                }
            }
        }
    }
    public void SetRoomWhenWin()
    {
        Vector2Int position = newPlayer.PosInMap;
        if (currentMapInstanceData.tileGrid.TryGetValue(position, out var tile))
        {
            if (!tile.visited)
            {
                tile.visited = true;
                if (!DataManager.Instance.GameData.VisitedTilePositions.Contains(position))
                {
                    DataManager.Instance.GameData.VisitedTilePositions.Add(position);
                }
            }
        }
    }


    private void GenerateSequenceMap()
    {
        if (currentMapInstanceData == null || currentMapInstanceData.ExitCount == 0)
        {
            Debug.LogError("Map hiện tại hoặc cửa thoát chưa được thiết lập đúng! Không thể tạo chuỗi map.");
            return;
        }

        int nextMapIndex = currentMapIndex + 1;

        if (nextMapIndex >= numFloor)
        {
            if (bossMaps.Count > 0)
            {
                var finalBossMap = bossMaps[^1];
                foreach (var exit in currentMapInstanceData.spawnedExitTriggers)
                {
                    exit.SubsequentMap = finalBossMap;
                }
            }
            return;
        }

        bool isBossFloor = nextMapIndex % 3 == 2;

        if (isBossFloor && bossMaps.Count > 1)
        {
            var nextBoss = bossMaps[2];
            foreach (var exit in currentMapInstanceData.spawnedExitTriggers)
            {
                exit.SubsequentMap = nextBoss;
            }
            return;
        }

        foreach (var exit in currentMapInstanceData.spawnedExitTriggers)
        {
            var nextMap = fightMaps[UnityEngine.Random.Range(0, fightMaps.Count)];
            exit.SubsequentMap = nextMap;
        }
    }

    public void ProceedToNextMap(MapData mapData)
    {
        if (mapData == null)
        {
            Debug.LogError("Next map is null");
            return;
        }

        currentMapIndex++;
        DataManager.Instance.GameData.CurrentFloor = currentMapIndex;
        DataManager.Instance.GameData.CurrentMapData = mapData;
        DataManager.Instance.GameData.VisitedTilePositions.Clear();

        if (currentMapIndex <= numFloor)
        {
            LoadMap(mapData);
        }
        else
        {
            ControlerUIInGame.Instance.FinishUI.SetActive(true);
            RoomInGameManager.Instance.IsFinishGame = true;
            DataManager.Instance.GameData.SetKeepPlayState(false);
        }
    }
}