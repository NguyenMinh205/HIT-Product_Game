using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ObjectBase
{
    public override void Attack(GameObject obj, int damage)
    {
        if (obj.TryGetComponent(out Enemy enemy))
        {
            enemy.ReceiverDamage(damage);
        }
    }
}
