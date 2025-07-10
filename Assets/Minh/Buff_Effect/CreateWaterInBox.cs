using System.Collections;
using UnityEngine;

public class CreateWaterInBox : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private float posYWaterTop;
    [SerializeField] private float distanceWater;
    [SerializeField] private float moveSpeed; // tốc độ nước di chuyển (unit/second)

    public string Name { get; private set; }
    public int Value { get; private set; }
    public int Duration { get; private set; }

    private Coroutine moveCoroutine;

    public void CreateWaterBox(int value, int duration)
    {
        Name = "create_water_in_box";
        Value = value;
        Duration = duration;
    }

    public void CreateWater()
    {
        CreateWaterBox(3, 3);

        if (water != null)
        {
            Vector3 targetPos = new Vector3(water.transform.position.x, posYWaterTop, water.transform.position.z);
            Debug.Log(targetPos);
            StartMoveWater(targetPos);
        }
    }

    public void ExecuteEffect()
    {
        if (Duration <= 0 || water == null) return;

        Vector3 currentPos = water.transform.position;
        Vector3 targetPos = new Vector3(currentPos.x, currentPos.y - distanceWater, currentPos.z);

        StartMoveWater(targetPos);

        Duration--;
    }

    private void StartMoveWater(Vector3 targetPos)
    {
        // Nếu đang di chuyển nước, dừng lại trước khi bắt đầu cái mới
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveWaterSmoothly(targetPos));
    }


    private IEnumerator MoveWaterSmoothly(Vector3 target)
    {
        while (Vector3.Distance(water.transform.position, target) > 0.01f)
        {
            water.transform.position = Vector3.MoveTowards(
                water.transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        water.transform.position = target; // đảm bảo đến đúng vị trí cuối cùng
    }


    public void Apply(Player player)
    {
        CreateWater();
    }

    public void Remove(Player player)
    {
        // Optional: làm nước biến mất nếu muốn
    }
}
