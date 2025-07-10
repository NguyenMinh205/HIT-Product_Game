using UnityEngine;

public class Thief : IBuffEffect
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Duration { get; set; }
    private Player player;

    public Thief(float value, float duration)
    {
        Name = "thief";
        Value = value; // Số lượng vàng hoặc vật phẩm bị ăn trộm mỗi lần
        Duration = duration; // Số lần quái có thể ăn trộm, -1 nếu vĩnh viễn
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
        // Đăng ký sự kiện OnTakeDamage để ăn trộm khi player nhận sát thương
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnTakeDamage, OnTakeDamage);
    }

    public void UnregisterEvents()
    {
        // Hủy đăng ký sự kiện khi hiệu ứng hết thời gian hoặc bị xóa thủ công
        ObserverManager<EventID>.RemoveAddListener(EventID.OnTakeDamage, OnTakeDamage);
    }

    private void OnTakeDamage(object param)
    {
        // Kiểm tra nếu Duration không phải vĩnh viễn (-1) và đã hết
        if (Duration != -1 && Duration <= 0)
        {
            Remove(player);
            return;
        }

        // Logic: Ăn trộm vàng hoặc vật phẩm từ player
        if (player.Inventory != null)
        {
            //player.Inventory.RemoveGold((int)Value); // Giả định có phương thức RemoveGold
            Debug.Log($"Thief stole {Value} gold! Duration: {(Duration == -1 ? "Permanent" : Duration.ToString())}");
        }

        // Giảm Duration nếu không phải vĩnh viễn
        if (Duration != -1)
        {
            Duration--;
        }
    }
}