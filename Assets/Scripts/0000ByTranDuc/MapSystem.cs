using System;
using System.Collections.Generic;
using TMPro;
using TranDuc;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : Singleton<MapSystem>
{
    #region Setup Map

    [SerializeField] private List<MapData> fightMaps;
    [SerializeField] private List<MapData> bossMaps;
    [SerializeField] private List<MapData> restMaps;
    [SerializeField] private TextMeshProUGUI floorTxt;
    [SerializeField] private TextMeshProUGUI floorInRoomTxt;
    [SerializeField] private int numFloor;

    public int NumFloor => numFloor;
    private int currentMapIndex = 0;
    public int MapIndex
    {
        get => this.currentMapIndex;
    }
    [SerializeField] private GameObject RoomVisual;

    [SerializeField] private PlayerInMap playerPrefab;
    [SerializeField] private CharacterDatabaseSO characterDatabase;
    [SerializeField] private Transform mapStore;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private GameObject currentMapInstance;
    private MapData currentMapData;
    private List<SpecialTile> specialTile = new List<SpecialTile>();
    public List<SpecialTile> SpecialTile => specialTile;
    #endregion
    PlayerInMap newPlayer;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (DataManager.Instance.GameData.IsKeepingPlayGame)
        {
            currentMapIndex = DataManager.Instance.GameData.CurrentFloor;
            LoadMap();
        }
        else
        {
            currentMapIndex = 0;
            LoadInitialMap();
        }
    }

    private void UpdateFloorText()
    {
        floorTxt.SetText("Floor " + currentMapIndex + "/" + numFloor);
        floorInRoomTxt.SetText("Floor " + currentMapIndex + "/" + numFloor);
    }
    public void SetActiveRoomVisual(bool val)
    {
        RoomVisual.SetActive(val);
        SetActiveMapStore(val);
        if (newPlayer == null) return;
        if (!val)
        {
            newPlayer.IsMoving = true;
            newPlayer.IsIntoRoom = true;
        }
        else
        {
            newPlayer.IsIntoRoom = false;
            newPlayer.IsMoving = false;
        }
    }
    private void LoadInitialMap()
    {
        if (fightMaps.Count == 0)
        {
            return;
        }
        currentMapIndex++;
        currentMapData = fightMaps[0];
        currentMapData.UpdateMapLayout();
        DataManager.Instance.GameData.CurrentFloor = currentMapIndex;
        DataManager.Instance.GameData.CurrentMapData = currentMapData;
        DataManager.Instance.GameData.SpecialTiles = specialTile;
        LoadMap(currentMapData);
        SetupMap();
        GenerateSequenceMap();
        UpdateFloorText();
        Debug.Log($"Initial map loaded: {currentMapData.MapType} at index {currentMapIndex}");
    }

    private void LoadMap()
    {
        currentMapIndex = DataManager.Instance.GameData.CurrentFloor;
        currentMapData = DataManager.Instance.GameData.CurrentMapData;
        if (specialTile != null) specialTile.Clear();
        specialTile = DataManager.Instance.GameData.SpecialTiles;
        if (currentMapData == null)
        {
            LoadInitialMap();
            return;
        }
        currentMapData.UpdateMapLayout();
        LoadMap(currentMapData);
        SetupMap();
        GenerateSequenceMap();
        UpdateFloorText();
    }
    private void SetActiveMapStore(bool val)
    {
        mapStore.gameObject.SetActive(val);
    }
    private void LoadMap(MapData mapData)
    {
        if (currentMapInstance != null)
        {
           Destroy(currentMapInstance.gameObject);
        }
        if (specialTile != null) specialTile.Clear();
        if (DataManager.Instance.GameData.IsKeepingPlayGame)
        {
            specialTile = DataManager.Instance.GameData.SpecialTiles;
        }
        else
        {
            foreach (var tile in currentMapData.MoveTiles)
            {
                if (tile != null)
                {
                    SpecialTile special = new SpecialTile
                    {
                        tileData = tile,
                        isInto = false,
                    };
                    specialTile.Add(special);
                }
            }
        }
            currentMapInstance = PoolingManager.Spawn(mapData.MapPrefab, this.transform.position - new Vector3(offsetX, offsetY, 0), Quaternion.identity, mapStore);
        currentMapData = mapData;
    }

    private void SetupMap()
    {
        Tilemap tilemap = currentMapInstance.GetComponentInChildren<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("No Tilemap found in map prefab!");
            return;
        }

        BoundsInt bounds = currentMapData.TilemapBounds;

        for (int x = 0; x < currentMapData.MapLayout.Count; x++)
        {
            for (int y = 0; y < currentMapData.MapLayout[x].Count; y++)
            {
                int gridX = x + bounds.xMin;
                int gridY = y + bounds.yMin;
                Vector3Int gridPos = new Vector3Int(gridX, gridY, 0);
                Vector3 worldPos = tilemap.CellToWorld(gridPos);
                Vector3 adjustedPos = new Vector3(worldPos.x + offsetX, worldPos.y + offsetY / 2, worldPos.z);


                var tileType = currentMapData.MapLayout[x][y];
                if (currentMapData.MoveTiles != null) return;
                if (tileType == EMapTileType.Entrance)
                {
                    {
                        foreach (var tile in currentMapData.MoveTiles)
                        {
                            if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Entrance)
                            {
                                if (tile.tileIcon != null)
                                {
                                    GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                }
                                break;
                            }
                        }
                    }

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
                    }
                }
                else if (tileType == EMapTileType.Exit)
                {
                    if (currentMapData.MoveTiles != null)
                    {
                        foreach (var tile in currentMapData.MoveTiles)
                        {
                            if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Exit)
                            {
                                if (tile.tileIcon != null)
                                {
                                    GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                    ExitTrigger exitTrigger = spawnedObject.GetComponent<ExitTrigger>();
                                    if (exitTrigger != null)
                                    {
                                        currentMapData.AddSpawnedExitTrigger(exitTrigger);
                                        Debug.Log("11111");
                                    }
                                    else
                                    {
                                        Debug.LogError($"Tile Exit tại {adjustedPos} (Grid: {x}, {y}) không có component ExitTrigger!");
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                else if(tileType != EMapTileType.Nothing)
                {
                    if (currentMapData.MoveTiles != null)
                    {
                        foreach (var tile in currentMapData.MoveTiles)
                        {
                            if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == tileType)
                            {
                                var special = specialTile?.Find(s => s.tileData.position == tile.position && s.tileData.tileType == tile.tileType);
                                if (special.isInto)
                                    break;

                                if (tile.tileIcon != null)
                                {
                                    GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                }
                                break;
                            }
                        }
                    }
                }

            }
        }
    }

    public void SetRoomVisited()
    {
        Vector2Int position = newPlayer.PosInMap;
        var tile = specialTile.Find(t => t.tileData.position == position);
        if (tile != null)
        {
            tile.isInto = true;
        }
        DataManager.Instance.GameData.SpecialTiles = specialTile;
        //GameData.Instance.SaveMainGameData();
    }

    private void GenerateSequenceMap()
    {
        if (currentMapData == null || currentMapData.ExitDoors == null || currentMapData.ExitDoors.Count == 0)
        {
            Debug.LogError("Map hiện tại hoặc cửa thoát chưa được thiết lập đúng! Không thể tạo chuỗi map.");
            // Đây có thể là trường hợp bình thường nếu một map không có cửa thoát, tùy thuộc vào thiết kế game
            return;
        }


        if (currentMapIndex == numFloor - 1)
        {
            // Tầng áp chót, dẫn đến Boss cuối
            if (bossMaps.Count > 0)
            {
                MapData finalBossMap = bossMaps[bossMaps.Count - 1];
                foreach (var exit in currentMapData.ExitDoors)
                {
                    if (exit != null)
                    {
                        exit.SubsequentMap = finalBossMap;
                    }
                }
            }
            else
            {
                Debug.LogError("Không có map Boss cuối nào!");
            }
            return;
        }

        int nextMapIndex = currentMapIndex + 1;
        bool isBossFloor = (nextMapIndex % 3 == 2); // Ví dụ: tầng 2, 5, 8... là tầng boss

        if (isBossFloor)
        {
            // Chọn map Boss cho tầng tiếp theo
            if (bossMaps.Count > 1) // Cần ít nhất 2 map boss nếu boss cuối là riêng biệt
            {
                // Tránh chọn boss cuối nếu đây không phải tầng cuối cùng
                MapData nextBossMap = bossMaps[UnityEngine.Random.Range(0, bossMaps.Count - 1)];
                foreach (var exit in currentMapData.ExitDoors)
                {
                    if (exit != null)
                    {
                        exit.SubsequentMap = nextBossMap;
                    }
                }
            }
            else
            {
                Debug.LogError("Không đủ map Boss để chọn cho tầng Boss!");
            }
        }
        else if (currentMapData.MapType == EMapType.Bossfight && currentMapData != bossMaps[bossMaps.Count - 1])
        {
            // Nếu map hiện tại là map Boss (nhưng không phải boss cuối) và có nhiều cửa thoát
            if (currentMapData.NumOfExitDoor > 1)
            {
                if (restMaps.Count > 0 && fightMaps.Count > 0)
                {
                    MapData restMap = restMaps[UnityEngine.Random.Range(0, restMaps.Count)];
                    MapData fightMap = fightMaps[UnityEngine.Random.Range(0, fightMaps.Count)];

                    // Gán ngẫu nhiên cửa thoát dẫn đến Rest hoặc Fight
                    int restExitIndex = UnityEngine.Random.Range(0, currentMapData.ExitDoors.Count);
                    currentMapData.ExitDoors[restExitIndex].SubsequentMap = restMap;

                    // Gán cửa thoát còn lại (nếu có)
                    if (currentMapData.ExitDoors.Count > 1)
                    {
                        int fightExitIndex = (restExitIndex + 1) % currentMapData.ExitDoors.Count;
                        currentMapData.ExitDoors[fightExitIndex].SubsequentMap = fightMap;
                    }
                    else
                    {
                        Debug.LogError($"Tầng {currentMapIndex} (Boss): Chỉ có 1 cửa thoát -> {restMap.MapType} cho tầng {nextMapIndex}");
                    }
                }
                else
                {
                    Debug.LogError("Không đủ map Rest hoặc Fight để tạo nhánh cho map Boss!");
                }
            }
            else // Chỉ có một cửa thoát từ map Boss
            {
                if (fightMaps.Count > 0)
                {
                    MapData nextFightMap = fightMaps[UnityEngine.Random.Range(0, fightMaps.Count)];
                    foreach (var exit in currentMapData.ExitDoors)
                    {
                        if (exit != null)
                        {
                            exit.SubsequentMap = nextFightMap;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Không có map Fight nào để nối từ map Boss!");
                }
            }
        }
        else // Các tầng Fight hoặc Rest thông thường
        {
            if (currentMapData.NumOfExitDoor > 1)
            {
                List<MapData> availableFightMaps = new List<MapData>(fightMaps);
                // Gán ngẫu nhiên các map Fight cho từng cửa thoát
                for (int i = 0; i < currentMapData.ExitDoors.Count; i++)
                {
                    if (availableFightMaps.Count > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, availableFightMaps.Count);
                        MapData nextFightMap = availableFightMaps[randomIndex];
                        if (currentMapData.ExitDoors[i] != null)
                        {
                            currentMapData.ExitDoors[i].SubsequentMap = nextFightMap;
                        }
                        availableFightMaps.RemoveAt(randomIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"Không đủ map Fight để gán cho tất cả các cửa thoát từ tầng {currentMapIndex}. Một số cửa có thể không được gán.");
                        break;
                    }
                }
            }
            else // Chỉ có một cửa thoát
            {
                if (fightMaps.Count > 0)
                {
                    MapData nextFightMap = fightMaps[UnityEngine.Random.Range(0, fightMaps.Count)];
                    foreach (var exit in currentMapData.ExitDoors)
                    {
                        if (exit != null)
                        {
                            exit.SubsequentMap = nextFightMap;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Không có map Fight nào!");
                }
            }
        }
    }

    public void ProceedToNextMap(MapData subsequentMap)
    {
        if (subsequentMap == null)
        {
            Debug.LogError("Loi1");
            return;
        }
        currentMapIndex++;
        DataManager.Instance.GameData.CurrentFloor = currentMapIndex;
        DataManager.Instance.GameData.CurrentMapData = subsequentMap;
        if (currentMapIndex <= numFloor)
        {
            currentMapData = subsequentMap;
            currentMapData.UpdateMapLayout();
            LoadMap(currentMapData);
            GenerateSequenceMap();
            UpdateFloorText();
        }
        else
        {
            Debug.LogError("Dungeon completed!");
            ControlerUIInGame.Instance.FinishUI.SetActive(true);
            RoomInGameManager.Instance.IsFinishGame = true;
            DataManager.Instance.GameData.SetKeepPlayState(false);
        }
    }

}
[Serializable]
public class SpecialTile
{
    public TileData tileData;
    public bool isInto;
}