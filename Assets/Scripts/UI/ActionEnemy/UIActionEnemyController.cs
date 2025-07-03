using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIActionEnemyController : Singleton<UIActionEnemyController>
{
    [Header("Sprite Action Enemy")]
    [SerializeField] private Sprite spriteAttack;
    [SerializeField] private Sprite spriteDefend;
    [SerializeField] private Sprite spriteBuff;
    [SerializeField] private Sprite spriteEffect;
    [SerializeField] private Sprite spriteHeal;

    [Space]
    [Header("Action Enemy")]
    [SerializeField] private List<UIActionEnemy> _uiActionEnemy;
    [SerializeField] private float distanceEnemyAndActionUI = 0.4f;

    public void InitActionToEnemy(Enemy enemy)
    {
        Debug.Log("Init Action To Enemy");
        for(int i=0;i<_uiActionEnemy.Count; i++)
        {
            if(!_uiActionEnemy[i].gameObject.activeSelf)
            {
                _uiActionEnemy[i].gameObject.SetActive(true);
                enemy.UIAction = _uiActionEnemy[i];
                SetPosUIAction(enemy);
                return;
            }
        }
    }

    public void InitUIAction(Enemy enemy, int indexAction)  // 
    {
        ProcedureActionEnemy procedure = enemy.actions[indexAction];
        for (int i = 0; i < procedure.actionEnemy.Count; i++)
        {
            TypeEnemyAction type = procedure.actionEnemy[i];
            switch(type)
            {
                case TypeEnemyAction.Attack:
                    enemy.UIAction.SetAction(spriteAttack, i, enemy.Damage);
                    break;

                case TypeEnemyAction.Defend:
                    enemy.UIAction.SetAction(spriteDefend, i, 0);
                    break;

                case TypeEnemyAction.Heal:
                    enemy.UIAction.SetAction(spriteHeal, i, 0);
                    break;

                case TypeEnemyAction.Effect:
                    enemy.UIAction.SetAction(spriteEffect, i, 0);
                    break;

                case TypeEnemyAction.Buff:
                    enemy.UIAction.SetAction(spriteBuff, i, 0);
                    break;
            }
        }
        for(int i=2; i >= procedure.actionEnemy.Count; i--)
        {
            enemy.UIAction.UnActionIndexEnemy(i);
        }
    }
    public void SetPosUIAction(Enemy enemy)
    {
        Debug.Log("Set Position for UI Action Enemy");
        Vector3 posEnemy = enemy.transform.position;

        float height = enemy.SpriteIdle.bounds.extents.y;
        Vector3 pos = posEnemy + Vector3.up * height + Vector3.up * distanceEnemyAndActionUI;

        enemy.UIAction.gameObject.transform.position = pos;
    }
    public void Execute(int index)
    {
       // _uiActionEnemy[index].UnShow();
    }
}
