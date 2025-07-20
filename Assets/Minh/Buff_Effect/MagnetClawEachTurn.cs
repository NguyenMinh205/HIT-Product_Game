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
        Value = value;
        Duration = duration;
    }

    public void Apply(Player player)
    {
        this.player = player;
        RegisterEvents();
    }

    public void Remove(Player player)
    {
        UnregisterEvents();
    }

    public void RegisterEvents()
    {
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
    }

    public void UnregisterEvents()
    {
        ObserverManager<EventID>.RemoveAddListener(EventID.OnStartPlayerTurn, OnStartPlayerTurn);
    }

    private void OnStartPlayerTurn(object param)
    {
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