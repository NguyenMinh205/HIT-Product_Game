using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private string idEnemy;
    private string nameEnemy;
    private int damage;

    [SerializeField] private Animator animator;
    [SerializeField] private Sprite spriteIDle;

    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;

    [SerializeField] private int armor;
    [SerializeField] private int armorIncreased;

    [SerializeField] private HealthBar health;
    [SerializeField] private UIActionEnemy uiActionEnemy;
    [SerializeField] private float distanceEnemyAndHealthBar;

    public List<ProcedureActionEnemy> actions;
    private int indexAction;

    public string ID
    {
        get => this.idEnemy;
    }
    public int Damage
    {
        get => this.damage;
        set => this.damage = value;
    }
    public int Armor
    {
        get => this.armor;
        set => this.armor = value;
    }
    public int ArmorIncreased
    {
        get => this.armorIncreased;
    }
    public int HP
    {
        get => this.currentHp;
        set => currentHp = value;
    }
    public int CurrentHp
    {
        get => maxHp;
    }
    public HealthBar Health
    {
        get => this.health;
        set => this.health = value;
    }
    public UIActionEnemy UIAction
    {
        get => uiActionEnemy;
        set => uiActionEnemy = value;
    }
    public Sprite SpriteIdle
    {
        get => spriteIDle;
    }

    private void Awake()
    {
        actions = new List<ProcedureActionEnemy>();
    }

    public void Init(EnemyData data, string id)
    {
        Debug.Log("Init Enemy");
        foreach (DataEnemy enemy in data.dataEnemy)
        {
            if (enemy.idEnemy == id)
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
        maxHp = data.hpEnemy;
        HP = data.hpEnemy;
        armorIncreased = data.armorIncreased;

        animator.runtimeAnimatorController = data.controller;
        spriteIDle = data.spriteEnemyIdle;

        actions = data.actions;
        indexAction = 0;

        //Set ActionUI Enemy
        UIActionEnemyController.Instance.InitActionToEnemy(this);
        UIActionEnemyController.Instance.InitUIAction(this, indexAction);

        UIHealthBarController.Instance.InitHealthBarToObjectBase(this);
    }
    public void CalulationPositionEnemy(Vector3 posSpawnEnemy)
    {
        float height = SpriteIdle.bounds.extents.y;
        Vector3 newPos = posSpawnEnemy + Vector3.up * height + Vector3.up * distanceEnemyAndHealthBar;

        transform.position = newPos;
    }

    public bool ReceiverDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        HP -= finalDamage;

        health.UpdateArmor(this);
        health.UpdateHp(this);

        if (HP <= 0)
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
        for (int i = 0; i < actions[indexAction].actionEnemy.Count; i++)
        {
            EnemyActionFactory.GetActionEnemy(actions[indexAction].actionEnemy[i], this);
            uiActionEnemy.UnActionIndexEnemy(i);
        }

        indexAction++;
        if (indexAction >= actions.Count)
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

