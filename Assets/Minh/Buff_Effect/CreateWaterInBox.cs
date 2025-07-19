using System.Collections;
using UnityEngine;

public class CreateWaterInBox : MonoBehaviour ,IBuffEffect
{
    [SerializeField] private GameObject water;
    [SerializeField] private float posYWaterTop;
    [SerializeField] private float distanceWater;
    [SerializeField] private float moveSpeed; // tốc độ nước di chuyển (unit/second)

    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }

    private Coroutine moveCoroutine;

    public CreateWaterInBox(float value, float duration)
    {
        Name = "create_water_in_box";
        Value = value;
        Duration = duration;

        CreateWater();
    }

    public void CreateWater()
    {

        if (water != null)
        {
            Vector3 targetPos = new Vector3(water.transform.position.x, posYWaterTop, water.transform.position.z);
            Debug.Log(targetPos);
            StartMoveWater(targetPos);
        }
    }

    public void ExecuteEffect(object obj)
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
        RegisterEvents();
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartPlayerTurn, ExecuteEffect);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartPlayerTurn, ExecuteEffect);
    }
}
