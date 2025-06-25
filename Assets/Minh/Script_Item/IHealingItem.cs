using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealingItem
{
    public abstract void Healing(Player player, float val);
}
