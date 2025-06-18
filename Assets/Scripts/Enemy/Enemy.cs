using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : ObjectBase
{
    [SerializeField] private List<Action> actions;
    private UIActionEnemy uiActionEnemy;

    public UIActionEnemy UI
    {
        get => uiActionEnemy;
        set => uiActionEnemy = value;
    }
    private void Awake()
    {
        actions = new List<Action>();
    }
    public override void Attack(GameObject obj, int damage)
    {
        if (obj.TryGetComponent(out Player player))
        {
            player.ReceiverDamage(damage);
        }
    }
    public void Shield()
    {
        base.Armor += 8;
        base.Info.UpdateArmor();
    }

    public void InitActionManager()
    {

        int actionQuantity = UnityEngine.Random.Range(1, 4);
        actions.Clear();

        for (int i=0; i < actionQuantity; i++)
        {
            int IsCheck = UnityEngine.Random.Range(1, 3);

            switch(IsCheck)
            {
                case 1:
                    actions.Add(() => Attack(GameController.Instance.playerController.CurrentPlayer.gameObject, 12));
                    uiActionEnemy.OnShowAttack(i, 12);
                    break;
                case 2:
                    actions.Add(() => Shield());
                    uiActionEnemy.OnShowShield(i);
                    break;
            }
        }
        for (int j = 2; j >= actionQuantity ; j--)
            uiActionEnemy.Execute(j);
    }

    public void ExecuteAction()
    {
        for(int i=0;i<actions.Count;i++)
        {
            actions[i].Invoke();
            uiActionEnemy.Execute(i);
        }
        actions.Clear();
    }
}
