using System.Collections;
using UnityEngine;

public enum WaterEffectType
{
    CreateWater,
    ExecuteWater
}
public class CreateWaterInBox : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private float posYWaterTop;
    [SerializeField] private float distanceWater;
    [SerializeField] private float moveSpeed; // tốc độ nước di chuyển (unit/second)

    private Coroutine moveCoroutine;

    private void Awake()
    {
        ObserverManager<WaterEffectType>.AddDesgisterEvent(WaterEffectType.CreateWater, CreateWater);
        ObserverManager<WaterEffectType>.AddDesgisterEvent(WaterEffectType.ExecuteWater, ExecuteEffect);
    }
    private void OnDisable()
    {
        ObserverManager<WaterEffectType>.RemoveAddListener(WaterEffectType.CreateWater, CreateWater);
        ObserverManager<WaterEffectType>.RemoveAddListener(WaterEffectType.ExecuteWater, ExecuteEffect);
    }

    public void CreateWater(object obj)
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
        if (water == null) return;

        Vector3 currentPos = water.transform.position;
        Vector3 targetPos = new Vector3(currentPos.x, currentPos.y - distanceWater, currentPos.z);

        StartMoveWater(targetPos);

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

}
