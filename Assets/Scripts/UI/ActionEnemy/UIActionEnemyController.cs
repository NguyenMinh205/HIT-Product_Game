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
    [SerializeField] private Sprite spriteDeBuff;
    [SerializeField] private Sprite spriteDrop;

    [Space]
    [Header("Action Enemy")]
    [SerializeField] private float distanceEnemyAndActionUI = 0.4f;

    public void InitActionToEnemy(Enemy enemy)
    {
        Debug.Log("Init Action To Enemy");

    }

    public void InitUIAction(Enemy enemy, int indexAction)  // 
    {
        ProcedureActionEnemy procedure = enemy.actions[indexAction];
        //Xu ly toa do UI Action Enemy
        int countAction = procedure.actionEnemy.Count;

        int posXActionUI = (countAction - 1) * -15;

        for (int i = 0; i < procedure.actionEnemy.Count; i++)
        {
            TypeEnemyAction type = procedure.actionEnemy[i];
            switch(type)
            {
                case TypeEnemyAction.Attack:
                    enemy.UIAction.SetAction(spriteAttack, posXActionUI, enemy.Damage, enemy);
                    break;

                case TypeEnemyAction.Defend:
                    enemy.UIAction.SetAction(spriteDefend, posXActionUI, 0, enemy);
                    break;

                case TypeEnemyAction.Heal:
                    enemy.UIAction.SetAction(spriteHeal, posXActionUI, 0, enemy);
                    break;

                case TypeEnemyAction.Effect:
                    enemy.UIAction.SetAction(spriteEffect, posXActionUI, 0, enemy);
                    break;

                case TypeEnemyAction.Buff:
                    enemy.UIAction.SetAction(spriteBuff, posXActionUI, 0, enemy);
                    break;

                case TypeEnemyAction.DeBuff:    
                    enemy.UIAction.SetAction(spriteDeBuff, posXActionUI, 0, enemy);
                    break;
                case TypeEnemyAction.Drop:
                    enemy.UIAction.SetAction(spriteDrop, posXActionUI, 0, enemy);
                    break;
            }
            posXActionUI += 30;
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
