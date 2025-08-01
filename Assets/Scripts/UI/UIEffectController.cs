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
                if (effectUI.effect.Duration == -1)
                {
                    effectUI.InitEffectUI(effect.Icon, effect.Value);
                }
                else
                {
                    effectUI.InitEffectUI(effect.Icon, effectUI.effect.Duration);
                }
                return;
            }
        }
        EffectUI newEffectUI = PoolingManager.Spawn(effectUIPrefab, transform.position, Quaternion.identity, effectGood);
        newEffectUI.effect = effect;
        effectUIs.Add(newEffectUI);
        if (newEffectUI.effect.Duration == -1)
        {
            newEffectUI.InitEffectUI(effect.Icon, effect.Value);
        }
        else
        {
            newEffectUI.InitEffectUI(effect.Icon, newEffectUI.effect.Duration);
        }

        RectTransform rect = newEffectUI.GetComponent<RectTransform>();
        rect.localPosition = new Vector3((index - 4) * 25f,0f,0f);
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
                if(effectUI.effect.Duration == -1)
                {
                    effectUI.InitEffectUI(effect.Icon, effect.Value);
                    CheckEffect();
                }
                else
                {
                    effectUI.InitEffectUI(effect.Icon, effectUI.effect.Duration);
                    CheckEffect();
                }
                return;
            }
        }
    }

    public void RemoveEffect(IBuffEffect effect)
    {
        Debug.Log($"Remove Effect: {effect.Name}");
        foreach (EffectUI effectUI in effectUIs)
        {
            if (effectUI.effect.Name == effect.Name)
            {
                effectUIs.Remove(effectUI);
                PoolingManager.Despawn(effectUI.gameObject);
                return;
            }
        }
    }
    public void CheckEffect()
    {
        Debug.Log("Check Effect");
        foreach (EffectUI effectUI in effectUIs)
        {
            if(effectUI.effect.Duration == -1)
            {
                if(effectUI.effect.Value <= 0)
                {
                    RemoveEffect(effectUI.effect);
                    return;
                }
            }
            else
            {
                if (effectUI.effect.Duration <= 0)
                {
                    RemoveEffect(effectUI.effect);
                    return;
                }
            }
        }
    }
    
    public void ClearAllEffectUI()
    {
        for (int i = effectUIs.Count - 1; i >= 0; i--)
        {
            if (effectUIs[i] != null)
            {
                PoolingManager.Despawn(effectUIs[i].gameObject);
            }
        }
        effectUIs.Clear();
    }

}
