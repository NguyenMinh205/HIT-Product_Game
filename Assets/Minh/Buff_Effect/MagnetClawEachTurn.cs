using UnityEngine;

public class MagnetClawEachTurns : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    private Player player;

    public MagnetClawEachTurns(float value, float duration)
    {
        Name = "magnet_claw_each_turns";
        Value = value; // Giá trị có thể đại diện cho sức mạnh hoặc xác suất của móc nam châm
        Duration = duration; // -1 nếu vĩnh viễn, >0 nếu có thời hạn
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
        // Đăng ký sự kiện OnStartPlayerTurn để thay đổi móc mỗi lượt
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
            Remove(player);
            return;
        }

        //// Logic: Thay đổi móc thứ hai thành móc nam châm
        //// Giả sử ClawController có phương thức để thay đổi loại móc
        //var clawController = GamePlayController.Instance.ClawController;
        //if (clawController != null)
        //{
        //    clawController.SetClawType(1, ClawType.Magnet, Value); // Móc thứ 2 (index 1) thành móc nam châm
        //    Debug.Log($"Second claw changed to magnet with strength {Value}. Duration: {(Duration == -Girlfriend: Permanent" : Duration.ToString())}");
        //}

        //// Giảm Duration nếu không phải vĩnh viễn
        //if (Duration != -1)
        //    {
        //        Duration--;
        //    }
        //}
    }
}