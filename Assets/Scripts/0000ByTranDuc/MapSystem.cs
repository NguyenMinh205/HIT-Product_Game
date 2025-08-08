
using DG.Tweening;
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
    private PlayerInMap curPlayerMap;
    public PlayerInMap CurPlayerMap => curPlayerMap;

    protected override void Awake() => base.Awake();

    private void Start()
    {
        if (DataManager.Instance.GameData.IsKeepingPlayGame)
        {
            currentMapIndex = DataManager.Instance.GameData.CurrentFloor;
            LoadMap(DataManager.Instance.GameData.CurrentMapData);
            DOVirtual.DelayedCall(0.25f, () =>
            {
                DataManager.Instance.GameData.SetKeepPlayState(false);
                ControllerUIInGame.Instance.UpdateNumOfCoinInMap(DataManager.Instance.GameData.Player.stats.Coin);
            });
        }
        else
        {
            currentMapIndex = 0;
            LoadInitialMap();
        }
    }

    public void SetActiveMapStore(bool val)
    {
        mapStore.gameObject.SetActive(val);
    }

    private void UpdateFloorText()
    {
        floorTxt.SetText($"Floor {currentMapIndex}/{numFloor}");
        floorInRoomTxt.SetText($"Floor {currentMapIndex}/{numFloor}");
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
            Vector3 adjustedPos = new Vector3(worldPos.x + offsetX, worldPos.y + 0.5f, worldPos.z);

            var tileType = tile.tileTypes;
            switch (tileType)
            {
                case EMapTileType.Empty:
                case EMapTileType.Nothing:
                    break;
                case EMapTileType.Entrance:
                    PoolingManager.Spawn(tile.iconPrefab, adjustedPos, Quaternion.identity, mapStore);
                    if(curPlayerMap != null) Destroy(curPlayerMap);
                    if (DataManager.Instance.GameData.IsKeepingPlayGame)
                    {
                        var posData = DataManager.Instance.GameData.PlayerNodePosition;
                        int posX = posData.x - gridPos.x;
                        int posY = posData.y - gridPos.y;
                        Debug.LogError("SetupPlayerSpawn");
                        curPlayerMap = Instantiate(playerPrefab, adjustedPos + new Vector3(posX, posY, 0), Quaternion.identity, mapStore);
                        curPlayerMap.Initialize(tilemap, currentMapInstanceData, posData,
                            characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId).skins[DataManager.Instance.GameData.SelectedSkinIndex].skin);
                        Debug.LogError("Player Spawned at: " + (adjustedPos + new Vector3(posX, posY, 0)));
                    }
                    else
                    {
                        Debug.LogError("SetupPlayerSpawn");
                        curPlayerMap = Instantiate(playerPrefab, adjustedPos, Quaternion.identity, mapStore);
                        curPlayerMap.Initialize(tilemap, currentMapInstanceData, gridPos,
                            characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId).skins[DataManager.Instance.GameData.SelectedSkinIndex].skin);
                        Debug.LogError("Player Spawned at: " + adjustedPos);
                    }
                    break;
                case EMapTileType.Exit:
                    GameObject exitObj = PoolingManager.Spawn(tile.iconPrefab, adjustedPos, Quaternion.identity, mapStore);
                    if (!exitObj.activeSelf)
                    {
                        exitObj.SetActive(true);
                    }
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
        Vector2Int position = curPlayerMap.PosInMap;
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
        Vector2Int position = curPlayerMap.PosInMap;
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
                var finalBossMap = bossMaps[bossMaps.Count - 1];
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
            var nextBoss = bossMaps[UnityEngine.Random.Range(0, bossMaps.Count - 1)];
            foreach (var exit in currentMapInstanceData.spawnedExitTriggers)
            {
                exit.SubsequentMap = nextBoss;
            }
            return;
        }

        if (currentMapInstanceData.sourceData.MapType == EMapType.Bossfight && currentMapInstanceData.sourceData != bossMaps[bossMaps.Count - 1])
        {
            if (currentMapInstanceData.spawnedExitTriggers.Count > 1)
            {
                if (restMaps.Count > 0 && fightMaps.Count > 0)
                {
                    MapData restMap = restMaps[UnityEngine.Random.Range(0, restMaps.Count)];
                    MapData fightMap = fightMaps[UnityEngine.Random.Range(0, fightMaps.Count)];

                    int restExitIndex = UnityEngine.Random.Range(0, currentMapInstanceData.spawnedExitTriggers.Count);
                    currentMapInstanceData.spawnedExitTriggers[restExitIndex].SubsequentMap = restMap;

                    int fightExitIndex = (restExitIndex + 1) % currentMapInstanceData.spawnedExitTriggers.Count;
                    currentMapInstanceData.spawnedExitTriggers[fightExitIndex].SubsequentMap = fightMap;
                }
            }
            return;
        }

        int mapIndexBefore = -1;
        foreach (var exit in currentMapInstanceData.spawnedExitTriggers)
        {
            int mapIndex = UnityEngine.Random.Range(0, fightMaps.Count);
            while (mapIndex == mapIndexBefore)
            {
                mapIndex = UnityEngine.Random.Range(0, fightMaps.Count);
            }
            var nextMap = fightMaps[mapIndex];
            exit.SubsequentMap = nextMap;
            mapIndexBefore = mapIndex;
        }
    }

    public void ProceedToNextMap(MapData mapData)
    {
        if (mapData == null)
        {
            Debug.LogError("Next map is null");
            return;
        }

        if (mapStore != null)
        {
            foreach (Transform child in mapStore)
            {
                if (child.gameObject.activeSelf)
                {
                    PoolingManager.Despawn(child.gameObject);
                }
            }
        }

        DOVirtual.DelayedCall(0.1f, () =>
        {
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
                ControllerUIInGame.Instance.FinishUI.SetActive(true);
                RoomInGameManager.Instance.IsFinishGame = true;
                DataManager.Instance.GameData.SetKeepPlayState(false);
            }
        });
    }
}