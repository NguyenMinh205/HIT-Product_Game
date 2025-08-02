using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coby : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        //Nhận hiệu ứng xóa bỏ một nửa số vật phẩm trong máy gắp, mỗi lần bỏ sẽ tăng damage gia tăng của player lên 1 giá trị
        UiPerksList.Instance.AddPerks(PerkIconManager.Instance.Coby);
    }

    public void StartSetupStat()
    {
        throw new System.NotImplementedException();
    }
}
