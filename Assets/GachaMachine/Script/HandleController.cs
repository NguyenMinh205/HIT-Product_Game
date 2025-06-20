using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Spin()
    {
        if (GachaMachine.Instance.State != GachaState.Start) return;
        StartCoroutine(SpinHandle());
    }

    private IEnumerator SpinHandle()
    {
        animator.SetBool("Spinning", true);
        yield return new WaitForSeconds(0.15f);
        GachaMachine.Instance.PullGacha();
        animator.SetBool("Spinning", false);
    }    
}
