using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected int curent_Hp;
    [SerializeField] protected int armor;

    [SerializeField] private InfoManager info;

    private int hp;

    private void Awake()
    {
        hp = curent_Hp;
    }
    private void OnEnable()
    {
        hp = curent_Hp;
    }
    public int Armor
    {
        get => this.armor;
        set => this.armor = value;
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
    public InfoManager Info
    {
        get => this.info;
        set => this.info = value;
    }
    public virtual void Attack(GameObject obj, int damage)
    {
    }
    public virtual void AttackAnimation()
    {

    }
    public virtual void ReceiverDamageAnimation()
    {

    }

    public virtual bool ReceiverDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);
        armor = Mathf.Max(0, armor - damage);

        hp -= finalDamage;

        info.UpdateArmor();
        info.UpdateHp();

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
        info.gameObject.SetActive(false);
    }
}
