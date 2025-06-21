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
    public override bool ReceiverDamage(int damage)
    {
        return base.ReceiverDamage(damage);
        Debug.Log("Player Receiver Damage");
        if(base.HP <= 0)
        {
            GameController.Instance.LoseGame();
        }
    }
}
