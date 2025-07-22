using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectController : MonoBehaviour
{
    [SerializeField] private List<EffectUI> effectUIs = new List<EffectUI>();
    [SerializeField] private EffectUI effectUIPrefab;

    [SerializeField] private Transform effectGood;
    [SerializeField] private Transform effectBad;

    public void InitEffect(int index, IBuffEffect effect)
    {
        foreach(EffectUI effectUI in effectUIs)
        {
            if (effectUI.effect.Name == effect.Name)
            {
                effectUI.effect.Duration += effect.Duration;
                effectUI.InitEffectUI(effect.Icon, effectUI.effect.Duration);
                return;
            }
        }
        EffectUI newEffectUI = PoolingManager.Spawn(effectUIPrefab, transform.position, Quaternion.identity, effectGood);
        newEffectUI.effect = effect;
        effectUIs.Add(newEffectUI);
        newEffectUI.InitEffectUI(effect.Icon, effect.Duration);

        RectTransform rect = newEffectUI.GetComponent<RectTransform>();
        rect.localPosition = new Vector3((index -2) * 37f,effectGood.position.y,0f);
    }

    public void SetEffect(IBuffEffect effect)
    {
        foreach (EffectUI effectUI in effectUIs)
        {
            if (effectUI.effect.Name == effect.Name)
            {
                //effectUI.effect.Duration --;
                if(effectUI.effect.Duration == 0)
                {
                    RemoveEffect(effect);
                    return;
                }
                effectUI.InitEffectUI(effect.Icon, effectUI.effect.Duration);
                return;
            }
        }
    }

    public void RemoveEffect(IBuffEffect effect)
    {
        foreach(EffectUI effectUI in effectUIs)
        {
            if (effectUI.effect.Name == effect.Name)
            {
                effectUIs.Remove(effectUI);
                PoolingManager.Despawn(effectUI.gameObject);
                return;
            }
        }
    }

    public void ClearAllEffectUI()
    {
        foreach (EffectUI effectUI in effectUIs)
        {
            PoolingManager.Despawn(effectUI.gameObject);
        }
    }
}
