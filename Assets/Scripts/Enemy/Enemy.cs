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
    public Animator Ani => animator;    
    [SerializeField] private Sprite spriteIdle;

    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;

    [SerializeField] private int armor;
    [SerializeField] private int armorIncreased;

    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private HealthBar health;
    [SerializeField] private UIActionEnemy uiActionEnemy;
    [SerializeField] private float offsetHealthBar = 1;

    public List<ProcedureActionEnemy> actions;
    private int indexAction;
    private float height;
    public int IndexAction => indexAction;

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
        set { this.armor = value; health.UpdateArmor(this); }
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
        get => spriteIdle;
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
        spriteIdle = data.spriteEnemyIdle;

        actions = data.actions;
        indexAction = 0;

        //Set ActionUI Enemy
        UIActionEnemyController.Instance.InitActionToEnemy(this);
        UIActionEnemyController.Instance.InitUIAction(this, indexAction);

        UIHealthBarController.Instance.InitHealthBarToObjectBase(this);
        enemySprite = gameObject.GetComponent<SpriteRenderer>();
        enemySprite.sprite = spriteIdle;
        height = enemySprite.bounds.size.y / 2;
        this.gameObject.transform.position += Vector3.up * height;
        health.transform.position = this.gameObject.transform.position - Vector3.up * (height + offsetHealthBar) ;
    }

    public bool ReceiverDamage(int damage)
    {
        Debug.Log("Enemy Receiver Damage : "+ damage);
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        HP -= finalDamage;

        if (HP < 0)
            HP = 0;

        //Chay Animation Enemy Receiver Damage
        ObserverManager<IDEnemyStateAnimation>.PostEven(IDEnemyStateAnimation.Hit, this);

        health.UpdateArmor(this);
        health.UpdateHp(this);

        if (HP < 0)
        {
            uiActionEnemy.UnShowActionEnemy();
            return true;
        }

        return false;
    }
    public void CheckDieEnemy()
    {
        if (HP <= 0)
        {
            //Goi khi enemy died
            ObserverManager<IDEnemyState>.PostEven(IDEnemyState.EnemyDied, this);
        }
    }

    public IEnumerator ExecuteAction()
    {
        for (int i = 0; i < actions[indexAction].actionEnemy.Count; i++)
        {
            EnemyActionFactory.GetActionEnemy(actions[indexAction].actionEnemy[i], this);
            uiActionEnemy.UnActionIndexEnemy(i);

            yield return new WaitForSeconds(1f);
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

