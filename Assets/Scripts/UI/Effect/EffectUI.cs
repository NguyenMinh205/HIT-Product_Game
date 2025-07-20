using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUI : MonoBehaviour
{
    public IBuffEffect effect;
    [SerializeField] private IconEffect icon;
    [SerializeField] private TextEffect text;

    public void InitEffectUI(Sprite sprite, float val)
    {
        icon.SetIcon(sprite);
        text.SetText(val);
    }
}
