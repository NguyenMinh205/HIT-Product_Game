using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkIconManager : Singleton<PerkIconManager>
{
    [Header("Character Ability")]
    [SerializeField] public Sprite Alex;
    [SerializeField] public Sprite Coby;
    [SerializeField] public Sprite Damian;
    [SerializeField] public Sprite Liam;
    [SerializeField] public Sprite Violet;

    [Space]
    [Header("Perks Icon")]
    [SerializeField] public Sprite perks;
}
