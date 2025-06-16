using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected int curent_Hp;
    [SerializeField] protected int damage;
    [SerializeField] protected int armor;

    private int hp;

    private void Start()
    {
        hp = curent_Hp;
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
    public int HP
    {
        get => this.hp;
    }
    public int CurrentHp
    {
        get => curent_Hp;
    }
    public virtual void Attack(GameObject obj)
    {
    }

    public virtual bool ReceiverDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        hp -= finalDamage;

        if (hp <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    public void Die()
    {
        PoolingManager.Despawn(gameObject);
    }

}
