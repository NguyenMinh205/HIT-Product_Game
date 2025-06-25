using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefendItem
{
    public abstract void Defend(Player player, float shield = 0);
}
