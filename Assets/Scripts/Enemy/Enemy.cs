using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : ObjectBase
{
    private string idEnemy;
    private string nameEnemy;
    private int damage;

    [SerializeField] private float distanceEnemyAndHealthBar;
    [SerializeField] private UIActionEnemy uiActionEnemy;

    public List<ProcedureActionEnemy> actions;
    private int indexAction;


    public UIActionEnemy UIAction
    {
        get => uiActionEnemy;
        set => uiActionEnemy = value;
    }

    public int Damage
    {
        get => this.damage;
        set => this.damage = value;
    }
    public string ID
    {
        get => this.idEnemy;
    }

    private void Awake()
    {
        actions = new List<ProcedureActionEnemy>();
    }
    public void Init(EnemyData data, string id)
    {
        Debug.Log("Init Enemy");
        foreach(DataEnemy enemy in data.dataEnemy)
        {
            if(enemy.idEnemy == id)
            {
                InitEnemyDetail(enemy);
                return;
            }
        }
    }
    public void InitEnemyDetail(DataEnemy data)
    {
        idEnemy = data.idEnemy;
        nameEnemy = data.nameEnemy;

        damage = data.damageEnemy;
        base.curent_Hp = data.hpEnemy;
        base.HP = data.hpEnemy;
        base.armorIncreased = data.armorIncreased;

        base.animator.runtimeAnimatorController = data.controller;
        base.spriteIDle = data.spriteEnemyIdle;

        actions = data.actions;
        indexAction = 0;

        //Set ActionUI Enemy
        UIActionEnemyController.Instance.InitActionToEnemy(this);
        UIActionEnemyController.Instance.InitUIAction(this, indexAction);

        UIHealthBarController.Instance.InitHealthBarToObjectBase(this);
    }
    public void CalulationPositionEnemy(Vector3 posSpawnEnemy)
    {
        float height = base.SpriteIdle.bounds.extents.y;
        Vector3 newPos = posSpawnEnemy + Vector3.up * height + Vector3.up * distanceEnemyAndHealthBar;

        transform.position = newPos;
    }

    public override bool ReceiverDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        base.HP -= finalDamage;

        base.Health.UpdateArmor(this);
        base.Health.UpdateHp(this);

        if (base.HP <= 0)
        {
            //Goi khi enemy died
            uiActionEnemy.UnShowActionEnemy();
            ObserverManager<IDEnemyState>.PostEven(IDEnemyState.EnemyDied, this);
            return true;
        }
        return false;
    }

    public void ExecuteAction()
    {
        for(int i=0; i < actions[indexAction].actionEnemy.Count; i ++)
        {
            EnemyActionFactory.GetActionEnemy(actions[indexAction].actionEnemy[i], this);
            uiActionEnemy.UnActionIndexEnemy(i);
        }

        indexAction++;
        if(indexAction >= actions.Count)
        {
            indexAction = 0;
        }
        //UIActionEnemyController.Instance.InitUIAction(this, indexAction);
    }

    public void NextAction()
    {
        UIActionEnemyController.Instance.InitUIAction(this, indexAction);
    }

}
