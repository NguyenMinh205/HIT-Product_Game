using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ObjectBase
{
    [SerializeField] private List<Action> actions;

    public override void Attack(GameObject obj)
    {
        if (obj.TryGetComponent(out Player player))
        {
            player.ReceiverDamage(base.damage);
        }
    }

    public void InitActionManager()
    {
        int actionQuantity = UnityEngine.Random.Range(1, 4);

        for(int i=0; i < actionQuantity; i++)
        {

        }
    }
}
