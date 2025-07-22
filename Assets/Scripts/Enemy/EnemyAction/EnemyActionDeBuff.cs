using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionDeBuff : MonoBehaviour
{
    public class GetPoison : IEnemyAction
    {
        public void Execute(Enemy enemy)
        {
            GamePlayController.Instance.PlayerController.CurrentPlayer.AddBuffEffect("poison_effect", 5, 3);
        }
    }
}
