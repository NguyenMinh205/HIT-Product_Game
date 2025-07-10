using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectManager
{
    EffectPlayer,
    EffectEnemy
}
public class PlayerEffectManager : MonoBehaviour
{
    public void RunEffectInPlayer()
    {
        ObserverManager<EffectManager>.PostEven(EffectManager.EffectPlayer);
    }
}
