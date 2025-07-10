using UnityEngine;

public class Vanish : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    private Player player;

    public Vanish(float value, float duration)
    {
        Name = "vanish";
        Value = value; // Có thể đại diện cho xác suất hoặc điều kiện bỏ chạy
        Duration = duration; // Số lượt trước khi quái trộm bỏ chạy, -1 nếu vĩnh viễn
    }

    public void Apply(Player player)
    {
        // Lưu tham chiếu đến player để sử dụng trong các sự kiện
        this.player = player;
        RegisterEvents();
    }

    public void Remove(Player player)
    {
        // Hủy đăng ký sự kiện khi hiệu ứng bị xóa
        UnregisterEvents();
    }

    public void RegisterEvents()
    {
        // Đăng ký sự kiện OnStartPlayerTurn để kiểm tra điều kiện bỏ chạy
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
    }

    public void UnregisterEvents()
    {
        // Hủy đăng ký sự kiện khi hiệu ứng hết thời gian hoặc bị xóa thủ công
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
    }

    private void OnStartPlayerTurn(object param)
    {
        // Kiểm tra nếu Duration không phải vĩnh viễn (-1) và đã hết
        if (Duration != -1 && Duration <= 0)
        {
            // Logic: Kích hoạt bỏ chạy cho quái trộm
            var enemyController = GamePlayController.Instance.EnemyController;
            if (enemyController != null)
            {
                //enemyController.TriggerEnemyEscape(Value); // Gọi phương thức giả định để quái trộm bỏ chạy
                Debug.Log($"Enemy attempts to vanish with chance {Value}!");
            }
            Remove(player);
            return;
        }

        // Log số lượt còn lại trước khi quái trộm bỏ chạy
        Debug.Log($"Vanish countdown... Duration: {(Duration == -1 ? "Permanent" : Duration.ToString())}");

        // Giảm Duration nếu không phải vĩnh viễn
        if (Duration != -1)
        {
            Duration--;
        }
    }
}