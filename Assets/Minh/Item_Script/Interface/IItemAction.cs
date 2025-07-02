using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemAction
{
    void Execute(GameObject player, GameObject target, float value = 0);
}
