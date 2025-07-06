using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : ObjectBase
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float distancePlayerAndHealthBar;
    [SerializeField] private RuntimeAnimatorController playerAnim;

    private Character character;
    private CharacterStatSO stats;
    public CharacterStatSO Stats => stats;
    private ICharacterAbility ability;
    [SerializeField] private CharacterStatModifier characterStatModifier;
    public CharacterStatModifier _CharacterStatModifier => characterStatModifier;
    private List<IBuffEffect> turnBasedEffects = new List<IBuffEffect>();
    private List<IBuffEffect> reactiveEffects = new List<IBuffEffect>();
    [SerializeField] private Inventory inventory;


    public Inventory Inventory
    {
        get => inventory;
    }

    public void Initialize(Character selectedCharacter, CharacterStatSO characterStatSO, int index)
    {
        character = selectedCharacter;
        if (character.skins.Count > index)
        {
            playerAnim = character.skins[index].anim;
            playerSprite.sprite = character.skins[index].skin;
        }
        else
        {
            Debug.LogWarning("Index skin không hợp lệ hoặc không có skin nào!");
        }

        stats = characterStatSO;
        if (stats == null)
        {
            Debug.LogError("CharacterStatSO không được cung cấp!");
            return;
        }

        if (characterStatModifier != null)
        {
            characterStatModifier.Stats = stats;
            characterStatModifier.CurPlayer = this;
        }
        else
        {
            Debug.LogWarning("CharacterStatModifier không được gán trong Inspector!");
        }

        ability = CharacterAbilityFactory.CreateAbility(character.id);
        if (ability != null && stats != null)
        {
            ability.StartSetup(this);
        }

        if (inventory != null)
        {
            foreach (ItemInventory item in PlayerManager.Instance.TotalInventory.Items)
            {
                inventory.AddItem(item.itemBase, (int)Math.Ceiling(item.quantity / 2.0));
            }
        }

        ApplyInitialTurnBasedEffects();
    }
    public void CalulationPositionPlayer(Vector3 posPlayer)
    {
        float height = playerSprite.bounds.extents.y;
        Vector3 newPos = posPlayer + Vector3.up * height + Vector3.up * distancePlayerAndHealthBar;

        transform.position = newPos;
    }

    public void AddBuffEffect(string effectName, float value, float duration)
    {
        IBuffEffect effect = BuffEffectFactory.CreateEffect(effectName, value, duration);
        if (effect != null)
        {
            if (effect.Type == BuffEffectType.Turn_BasedEffects)
            {
                turnBasedEffects.Add(effect);
            }
            else if (effect.Type == BuffEffectType.ReactiveEffects)
            {
                reactiveEffects.Add(effect);
            }
        }
    }

    private void ApplyInitialTurnBasedEffects()
    {
        if (turnBasedEffects.Count == 0) return;
        foreach (IBuffEffect effect in turnBasedEffects)
        {
            effect.Apply(this);
        }
    }

    public void ApplyReactiveEffect(string effectName)
    {
        if (reactiveEffects.Count == 0) return;
        foreach (IBuffEffect effect in reactiveEffects)
        {
            if (effect.Name.ToLower() == effectName.ToLower())
            {
                effect.Apply(this);
                break;
            }
        }
    }

    public void AddItem()
    {
        foreach (ItemInventory item in PlayerManager.Instance.TotalInventory.Items)
        {
            //Check xem đã có đủ item chưa, item ấy có đủ rồi thì next
            inventory.AddItem(item.itemBase, (int)Math.Ceiling((item.quantity) / 2.0));
        }
    }

    public void RemoveItem()
    {

    }    

    private void RemoveInitialTurnBasedEffects(IBuffEffect effect)
    {
        turnBasedEffects.Remove(effect);
    }
    
    private void RemoveReactiveEffect(IBuffEffect effect)
    {
        reactiveEffects.Remove(effect);
    }


    public void ReceiveDamage(int damage)
    {
        if (characterStatModifier != null && characterStatModifier.Stats != null)
        {
            float effectiveDamage = damage - characterStatModifier.Stats.shield;
            effectiveDamage = Mathf.Max(0, effectiveDamage);
            characterStatModifier.Stats.currentHP -= effectiveDamage;

            Debug.Log($"Player nhận {effectiveDamage} damage, HP còn lại: {characterStatModifier.Stats.currentHP}");
            if (characterStatModifier.Stats.currentHP <= 0)
            {
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        Debug.Log("Game Over for Player");
    }
}