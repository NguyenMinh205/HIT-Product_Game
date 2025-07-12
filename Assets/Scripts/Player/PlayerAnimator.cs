using System.Collections;
using UnityEngine;

public enum IDStateAnimationPlayer
{
    Attack,
    Buff,
    Hit,
}
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float timeDelayAttack = 1f;
    [SerializeField] private float timeDelayBuff = 1f;
    [SerializeField] private float timeDelayHit = 1f;
    private bool isTrueAnimation = false;

    private void OnEnable()
    {
        ObserverManager<IDStateAnimationPlayer>.AddDesgisterEvent(IDStateAnimationPlayer.Attack, PlayAttack);
        ObserverManager<IDStateAnimationPlayer>.AddDesgisterEvent(IDStateAnimationPlayer.Buff, PlayBuff);
        ObserverManager<IDStateAnimationPlayer>.AddDesgisterEvent(IDStateAnimationPlayer.Hit, PlayHit);
    }
    private void OnDisable()
    {
        ObserverManager<IDStateAnimationPlayer>.RemoveAddListener(IDStateAnimationPlayer.Attack, PlayAttack);
        ObserverManager<IDStateAnimationPlayer>.RemoveAddListener(IDStateAnimationPlayer.Buff, PlayBuff);
        ObserverManager<IDStateAnimationPlayer>.RemoveAddListener(IDStateAnimationPlayer.Hit, PlayHit);
    }

    public void InitAnimator(RuntimeAnimatorController run)
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = run;
    }

    public void PlayAttack(object obj)
    {
        if(!isTrueAnimation)
            StartCoroutine(PlayAnimation("IsAttacking", timeDelayAttack));
    }

    public void PlayBuff(object obj)
    {
        if (!isTrueAnimation)
            StartCoroutine(PlayAnimation("IsBuffing", timeDelayBuff));
    }

    public void PlayHit(object obj)
    {
        if (!isTrueAnimation)
            StartCoroutine(PlayAnimation("IsHit", timeDelayHit));
    }

    private IEnumerator PlayAnimation(string boolName, float delay)
    {
        animator.SetBool(boolName, true);
        isTrueAnimation = true;
        yield return new WaitForSeconds(delay);
        animator.SetBool(boolName, false);
        isTrueAnimation = false;
    }
}