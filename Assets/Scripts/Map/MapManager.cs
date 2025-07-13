using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private List<MapData> fightMaps;
    [SerializeField] private List<MapData> bossMaps;
    [SerializeField] private List<MapData> restMaps;
    [SerializeField] private TextMeshProUGUI floorTxt;
    [SerializeField] private TextMeshProUGUI floorInRoomTxt;
    [SerializeField] private int numFloor;
    private MapData curMap;
    private int currentMapIndex = 0;

    public int MapIndex
    {
        get => this.currentMapIndex;   // lay ra index map de set spawn enemy
    }

    [SerializeField] private GameObject RoomVisual;

    private void Start()
    {
        fightMaps = Resources.LoadAll<MapData>("SO/Fight").ToList();
        bossMaps = Resources.LoadAll<MapData>("SO/Boss").ToList();
        restMaps = Resources.LoadAll<MapData>("SO/Rest").ToList();
        LoadInitialMap();
    }

    private void UpdateFloorText()
    {
        floorTxt.SetText("Floor " + currentMapIndex + "/" + numFloor);
        floorInRoomTxt.SetText("Floor " + currentMapIndex + "/" + numFloor);
    }
    public void SetActiveRoomVisual(bool val)
    {
        RoomVisual.SetActive(val);
    }
    private void LoadInitialMap()
    {
        if (fightMaps.Count == 0)
        {
            Debug.LogError("No fight maps available!");
            return;
        }
        currentMapIndex++;
        curMap = fightMaps[5];
        curMap.UpdateMapLayout(); // Xóa danh sách ExitDoors cũ và cập nhật layout
        MapController.Instance.LoadMap(curMap); // Spawn các đối tượng và thêm ExitTrigger vào curMap.ExitDoors
        GenerateSequenceMap(); // Bây giờ, curMap.ExitDoors sẽ chứa các ExitTrigger đã spawn và sẵn sàng để gán SubsequentMap
        UpdateFloorText();
        Debug.Log($"Initial map loaded: {curMap.MapType} at index {currentMapIndex}");
    }

    private void GenerateSequenceMap()
    {
        if (curMap == null || curMap.ExitDoors == null || curMap.ExitDoors.Count == 0)
        {
            Debug.LogError("Map hiện tại hoặc cửa thoát chưa được thiết lập đúng! Không thể tạo chuỗi map.");
            // Đây có thể là trường hợp bình thường nếu một map không có cửa thoát, tùy thuộc vào thiết kế game
            return;
        }

        Debug.Log($"Đang tạo chuỗi map cho tầng {currentMapIndex}, loại: {curMap.MapType}, số cửa thoát: {curMap.NumOfExitDoor}");

        if (currentMapIndex == numFloor - 1)
        {
            // Tầng áp chót, dẫn đến Boss cuối
            if (bossMaps.Count > 0)
            {
                MapData finalBossMap = bossMaps[bossMaps.Count - 1];
                foreach (var exit in curMap.ExitDoors)
                {
                    if (exit != null)
                    {
                        exit.SubsequentMap = finalBossMap;
                        Debug.Log($"Tầng {currentMapIndex}: Cửa thoát dẫn đến map Boss cuối: {finalBossMap.MapType}");
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
                MapData nextBossMap = bossMaps[Random.Range(0, bossMaps.Count - 1)];
                foreach (var exit in curMap.ExitDoors)
                {
                    if (exit != null)
                    {
                        exit.SubsequentMap = nextBossMap;
                        Debug.Log($"Tầng {currentMapIndex}: Cửa thoát dẫn đến map Boss: {nextBossMap.MapType} cho tầng {nextMapIndex}");
                    }
                }
            }
            else
            {
                Debug.LogError("Không đủ map Boss để chọn cho tầng Boss!");
            }
        }
        else if (curMap.MapType == EMapType.Bossfight && curMap != bossMaps[bossMaps.Count - 1])
        {
            // Nếu map hiện tại là map Boss (nhưng không phải boss cuối) và có nhiều cửa thoát
            if (curMap.NumOfExitDoor > 1)
            {
                if (restMaps.Count > 0 && fightMaps.Count > 0)
                {
                    MapData restMap = restMaps[Random.Range(0, restMaps.Count)];
                    MapData fightMap = fightMaps[Random.Range(0, fightMaps.Count)];

                    // Gán ngẫu nhiên cửa thoát dẫn đến Rest hoặc Fight
                    int restExitIndex = Random.Range(0, curMap.ExitDoors.Count);
                    curMap.ExitDoors[restExitIndex].SubsequentMap = restMap;

                    // Gán cửa thoát còn lại (nếu có)
                    if (curMap.ExitDoors.Count > 1)
                    {
                        int fightExitIndex = (restExitIndex + 1) % curMap.ExitDoors.Count; // Đảm bảo chọn cửa khác
                        curMap.ExitDoors[fightExitIndex].SubsequentMap = fightMap;
                        Debug.Log($"Tầng {currentMapIndex} (Boss): Cửa {restExitIndex} -> {restMap.MapType}, Cửa {fightExitIndex} -> {fightMap.MapType} cho tầng {nextMapIndex}");
                    }
                    else
                    {
                        Debug.Log($"Tầng {currentMapIndex} (Boss): Chỉ có 1 cửa thoát -> {restMap.MapType} cho tầng {nextMapIndex}");
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
                    MapData nextFightMap = fightMaps[Random.Range(0, fightMaps.Count)];
                    foreach (var exit in curMap.ExitDoors)
                    {
                        if (exit != null)
                        {
                            exit.SubsequentMap = nextFightMap;
                            Debug.Log($"Tầng {currentMapIndex} (Boss): Cửa thoát dẫn đến map Fight: {nextFightMap.MapType} cho tầng {nextMapIndex}");
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
            if (curMap.NumOfExitDoor > 1)
            {
                List<MapData> availableFightMaps = new List<MapData>(fightMaps);
                // Gán ngẫu nhiên các map Fight cho từng cửa thoát
                for (int i = 0; i < curMap.ExitDoors.Count; i++)
                {
                    if (availableFightMaps.Count > 0)
                    {
                        int randomIndex = Random.Range(0, availableFightMaps.Count);
                        MapData nextFightMap = availableFightMaps[randomIndex];
                        if (curMap.ExitDoors[i] != null)
                        {
                            curMap.ExitDoors[i].SubsequentMap = nextFightMap;
                            Debug.Log($"Tầng {currentMapIndex}: Cửa thoát {i} dẫn đến map Fight: {nextFightMap.MapType} cho tầng {nextMapIndex}");
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
                    MapData nextFightMap = fightMaps[Random.Range(0, fightMaps.Count)];
                    foreach (var exit in curMap.ExitDoors)
                    {
                        if (exit != null)
                        {
                            exit.SubsequentMap = nextFightMap;
                            Debug.Log($"Tầng {currentMapIndex}: Cửa thoát dẫn đến map Fight: {nextFightMap.MapType} cho tầng {nextMapIndex}");
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
        if (currentMapIndex <= numFloor)
        {
            curMap = subsequentMap;
            curMap.UpdateMapLayout();
            MapController.Instance.LoadMap(curMap); 
            GenerateSequenceMap();
            UpdateFloorText();
        }
        else
        {
            Debug.Log("Dungeon completed!");
            // Thêm logic xử lý khi hoàn thành dungeon ở đây
        }
    }
}