using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitTrigger : MonoBehaviour
{
    private MapData _subsequentMap;
    public MapData SubsequentMap
    {
        get => _subsequentMap;
        set
        {
            _subsequentMap = value;
            if (_subsequentMap != null)
            {
                UpdateIconMaps();
            }
        }
    }
    [SerializeField] private Transform listIconMapStore;
    [SerializeField] private Image iconPrefab;
    private List<Sprite> iconMaps = new List<Sprite>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PoolingManager.Despawn(other.gameObject);
            MapManager.Instance.ProceedToNextMap(SubsequentMap);
        }
    }

    public void UpdateIconMaps()
    {
        iconMaps.Clear();

        foreach (Transform child in listIconMapStore)
        {
            Destroy(child.gameObject);
        }

        if (SubsequentMap == null || SubsequentMap.MoveTiles == null)
        {
            Debug.LogWarning("SubsequentMap hoặc MoveTiles không được thiết lập!");
            return;
        }

        if (iconPrefab == null)
        {
            Debug.LogWarning("IconPrefab không được gán trong Inspector!");
            return;
        }

        foreach (var tileData in SubsequentMap.MoveTiles)
        {
            if (tileData == null || tileData.tileType == EMapTileType.Entrance || tileData.tileType == EMapTileType.Exit)
            {
                continue;
            }

            if (tileData.tileIcon != null)
            {
                SpriteRenderer spriteRenderer = tileData.tileIcon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null)
                {
                    iconMaps.Add(spriteRenderer.sprite);
                    Debug.Log($"Đã thêm sprite của tile {tileData.tileType} vào iconMaps tại vị trí ({tileData.position.x}, {tileData.position.y})");
                }
                else
                {
                    Debug.LogWarning($"TileIcon tại vị trí ({tileData.position.x}, {tileData.position.y}) không có SpriteRenderer hoặc sprite!");
                }
            }
            else
            {
                Debug.LogWarning($"TileIcon tại vị trí ({tileData.position.x}, {tileData.position.y}) là null!");
            }
        }

        foreach (var sprite in iconMaps)
        {
            Image imageInstance = Instantiate(iconPrefab, listIconMapStore);
            imageInstance.sprite = sprite;
            imageInstance.gameObject.SetActive(true);
            Debug.Log($"Đã spawn Image UI từ iconPrefab với sprite {sprite.name} trong listIconMapStore");
        }

        Debug.Log($"Tổng số sprite trong iconMaps: {iconMaps.Count}");
    }
}