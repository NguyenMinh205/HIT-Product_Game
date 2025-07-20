using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionousSpore : AttackWithEffect
{
    private int damage = 5;
    public int Damage { get { return damage; } set { damage = value; } }
    private int effectVal = 3;
    public int EffectVal => effectVal;
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
    }

    public override void Effect(Enemy enemy)
    {
        //Gây effectVal stack độc lên kẻ địch nhân đòn đánh
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Debug.Log("Player is not null, applying poison effect.");
            GamePlayController.Instance.PlayerController.CurrentPlayer.AddBuffEffect("poison_effect", 3,3);
        }
    }

    public override void Upgrade()
    {
    }
}
