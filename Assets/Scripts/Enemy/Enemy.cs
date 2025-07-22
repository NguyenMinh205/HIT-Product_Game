using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyEffect
{
    Remove,
}
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

    private List<IBuffEffect> activeEffects = new List<IBuffEffect>();
    [SerializeField] private UIEffectController effectController;
    private ItemBase itemEffect;
    public ItemBase ItemEffect => itemEffect;


    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private HealthBar health;
    [SerializeField] private UIActionEnemy uiActionEnemy;
    [SerializeField] private float offsetHealthBar;


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
        ObserverManager<EnemyEffect>.AddDesgisterEvent(EnemyEffect.Remove, RemoveEffect);
    }
    private void OnDisable()
    {
        ObserverManager<EnemyEffect>.RemoveAddListener(EnemyEffect.Remove, RemoveEffect);
    }
    public void InitEnemy(EnemyData data, string id)
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
    public void InitBoss(EnemyData data, string id)
    {
        Debug.Log("Init Boss");
        foreach (DataEnemy enemy in data.dataBoss)
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

        if(data.itemEffect != null)
            itemEffect = data.itemEffect;

        actions = new List<ProcedureActionEnemy>(data.actions);
        indexAction = 0;

        //UIActionEnemyController.Instance.InitActionToEnemy(this);
        //UIActionEnemyController.Instance.InitUIAction(this, indexAction);
        health.InitHealthBar(this);

        //UIHealthBarController.Instance.InitHealthBarToObjectBase(this);
        enemySprite = gameObject.GetComponent<SpriteRenderer>();
        enemySprite.sprite = spriteIdle;
        height = enemySprite.bounds.size.y / 2;
        Debug.Log("Height of Enemy: " + height);
        this.gameObject.transform.position += Vector3.up * height;

        RectTransform rectUIAction = uiActionEnemy.GetComponent<RectTransform>();
        rectUIAction.position = this.transform.position + Vector3.up * height + Vector3.up * offsetHealthBar;

        UIActionEnemyController.Instance.InitUIAction(this, indexAction);
    }

    public bool ReceiverDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        HP -= finalDamage;

        if (HP < 0)
            HP = 0;

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
            ObserverManager<IDEnemyState>.PostEven(IDEnemyState.EnemyDied, this);
            effectController.ClearAllEffectUI();
        }
    }

    public IEnumerator ExecuteAction()
    {
        ExecuteEffect();
        for (int i = 0; i < actions[indexAction].actionEnemy.Count; i++)
        {
            EnemyActionFactory.GetActionEnemy(actions[indexAction].actionEnemy[i], this);
            ObserverManager<EventID>.PostEven(EventID.OnDealDamage, this);
            uiActionEnemy.UnActionIndexEnemy(i);

            yield return new WaitForSeconds(1f);
        }
        uiActionEnemy.ClearAllActionList();
        indexAction++;
        if (indexAction >= actions.Count)
        {
            indexAction = 0;
        }
        //UIActionEnemyController.Instance.InitUIAction(this, indexAction);
    }
    public void AddBuffEffect(string effectName, float value, float duration)
    {
        IBuffEffect effect = BuffEffectFactory.CreateEffect(effectName, value, duration);
        if (effect == null) return;

        IBuffEffect existingEffect = GetActiveEffect(effectName);
        if (existingEffect != null)
        {
            if (existingEffect.Duration != -1 && duration != -1)
            {
                existingEffect.Duration += duration;
                Debug.Log($"Effect {effectName} duration stacked. New duration: {existingEffect.Duration}");
            }
            else if (duration == -1)
            {
                existingEffect.Duration = -1;
                Debug.Log($"Effect {effectName} set to permanent.");
            }
            return;
        }

        effect.ApplyEnemy(this);
        activeEffects.Add(effect);
        effectController.InitEffect(activeEffects.Count, effect);
        Debug.Log($"Applied new effect {effectName} with value {value} and duration {duration}.");
    }

    public IBuffEffect GetActiveEffect(string effectName)
    {
        return activeEffects.Find(effect => effect.Name.ToLower() == effectName.ToLower());
    }

    public void RemoveBuffEffect(IBuffEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            //effect.Remove(this);
            activeEffects.Remove(effect);
        }
    }
    public void ExecuteEffect()
    {
        ObserverManager<EventID>.PostEven(EventID.OnStartEnemyTurn, this);
        foreach (IBuffEffect effect in activeEffects)
        {
            effectController.SetEffect(effect);
        }
    }
    public void NextAction()
    {
        UIActionEnemyController.Instance.InitUIAction(this, indexAction);
    }

    public void RemoveEffect(object obj )
    {
        IBuffEffect effect = obj as IBuffEffect;
        if (activeEffects.Contains(effect))
        {
            effect.RemoveEnemy(this);
            activeEffects.Remove(effect);
            //effectController.RemoveEffect(effect);
            Debug.Log($"Removed effect {effect.Name} from enemy.");
        }
    }   

}

