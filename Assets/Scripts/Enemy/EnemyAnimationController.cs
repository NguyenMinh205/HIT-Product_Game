using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IDEnemyStateAnimation
{
    Attack,
    Buff,
    Hit
}

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private float timeDelayAttack = 1f;
    [SerializeField] private float timeDelayBuff = 1f;
    [SerializeField] private float timeDelayHit = 1f;
    private bool isTrueAnimation = false;

    private void OnEnable()
    {
        ObserverManager<IDEnemyStateAnimation>.AddDesgisterEvent(IDEnemyStateAnimation.Attack, PlayAttack);
        ObserverManager<IDEnemyStateAnimation>.AddDesgisterEvent(IDEnemyStateAnimation.Buff, PlayBuff);
        ObserverManager<IDEnemyStateAnimation>.AddDesgisterEvent(IDEnemyStateAnimation.Hit, PlayHit);
    }
    private void OnDisable()
    {
        ObserverManager<IDEnemyStateAnimation>.RemoveAddListener(IDEnemyStateAnimation.Attack, PlayAttack);
        ObserverManager<IDEnemyStateAnimation>.RemoveAddListener(IDEnemyStateAnimation.Buff, PlayBuff);
        ObserverManager<IDEnemyStateAnimation>.RemoveAddListener(IDEnemyStateAnimation.Hit, PlayHit);
    }

    public void PlayAttack(object obj)
    {
        if (!isTrueAnimation && obj is Enemy enemy)
            StartCoroutine(PlayAnimation("IsAttacking", timeDelayAttack, enemy));
    }

    public void PlayBuff(object obj)
    {
        if (!isTrueAnimation && obj is Enemy enemy)
            StartCoroutine(PlayAnimation("IsBuffing", timeDelayBuff, enemy));
    }

    public void PlayHit(object obj)
    {
        if (!isTrueAnimation && obj is Enemy enemy)
            StartCoroutine(PlayAnimation("IsHit", timeDelayHit, enemy));
    }

    private IEnumerator PlayAnimation(string boolName, float delay, Enemy enemy)
    {
        enemy.Ani.SetBool(boolName, true);
        isTrueAnimation = true;
        yield return new WaitForSeconds(delay);

        if(boolName == "IsHit")
        {
            enemy = GetComponent<Enemy>();
            enemy.CheckDieEnemy();
        }
        enemy.Ani.SetBool(boolName, false);
        isTrueAnimation = false;
    }
}
