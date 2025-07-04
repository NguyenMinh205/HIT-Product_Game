using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected Sprite spriteIDle;

    [SerializeField] protected int curent_Hp;
    private int hp;

    [SerializeField] protected int armor;
    [SerializeField] protected int armorIncreased;

    [SerializeField] private HealthBar health;

    private bool isPoison;
    private int poisonDamage;
    private void Awake()
    {
        hp = curent_Hp;
    }
    private void OnEnable()
    {
        hp = curent_Hp;
    }
    public bool IsPosion
    {
        get => isPoison;
        set => isPoison = value;
    }
    public int PoisonDamage
    {
        get => poisonDamage;
        set => poisonDamage = value;
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
        get => this.hp;
        set => hp = value;
    }
    public int CurrentHp
    {
        get => curent_Hp;
    }
    public HealthBar Health
    {
        get => this.health;
        set => this.health = value;
    }
    public Sprite SpriteIdle
    {
        get => spriteIDle;
    }
    public virtual void Attack(GameObject obj, int damage)
    {
    }
    public virtual void AttackAnimation()
    {

    }
    public virtual void CheckIsPoison()
    {
        if(isPoison)
        {
            ReceiverDamage(poisonDamage);
            poisonDamage = 0;
            isPoison = false;
        }
    }
    public virtual void ReceiverDamageAnimation()
    {

    }

    public virtual bool ReceiverDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        hp -= finalDamage;

        health.UpdateArmor(this);
        health.UpdateHp(this);

        if (hp <= 0)
        {
            EndGame();
            return true;
        }
        return false;
    }
    public void EndGame()
    {
        PoolingManager.Despawn(gameObject);
        health.gameObject.SetActive(false);
    }
}
