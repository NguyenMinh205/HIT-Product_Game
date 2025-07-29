using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private string name;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animation;
    public void Init(NPC_data data)
    {
        id = data.id;
        name = data.name;
        spriteRenderer.sprite = data.sprite;
        animation.runtimeAnimatorController = data.animatorController;
    }
}
