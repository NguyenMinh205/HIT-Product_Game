using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float offsetHealthBar = 1f;
    [SerializeField] private HealthBar health;

    private Character character;
    private CharacterStat stats;
    public CharacterStat Stats => stats;

    private List<IBuffEffect> activeEffects = new List<IBuffEffect>();
    public List<IBuffEffect> ActiveEffects => activeEffects;
    [SerializeField] private UIEffectController effectController;
    [SerializeField] private Inventory inventory;
    private List<ItemInventory> addedItems = new List<ItemInventory>();

    public bool IsDodge { get; set; }
    public bool IsCounterAttack { get; set; }

    public Inventory Inventory => inventory;
    public List<ItemInventory> AddedItems => addedItems;

    public HealthBar Health
    {
        get => health;
        set => health = value;
    }

    public void Initialize(Character selectedCharacter, CharacterStat characterStatSO, int index)
    {
        character = selectedCharacter;
        if (character.skins.Count > index)
        {
            playerAnimator.InitAnimator(character.skins[index].anim);
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

        if (inventory != null)
        {
            foreach (ItemInventory item in GamePlayController.Instance.PlayerController.TotalInventory.Items)
            {
                inventory.AddItem(item.itemId, (int)Math.Ceiling(item.quantity / 2.0), item.quantity, item.isUpgraded);
            }
        }
        IsDodge = false;
        IsCounterAttack = false;

        //activeEffects = new List<IBuffEffect>();
        health.InitHealthBar(this);
        ClearAllEffects();
    }

    public void AddBuffEffect(string effectName, float value, float duration)
    {
        IBuffEffect effect = BuffEffectFactory.CreateEffect(effectName, value, duration);
        if (effect == null) return;

        IBuffEffect existingEffect = GetActiveEffect(effectName);
        if (existingEffect != null)
        {
            if (existingEffect.Duration != -1 && duration != -1)
            {
                existingEffect.Duration += duration;
                Debug.Log($"Effect {effectName} duration stacked. New duration: {existingEffect.Duration}");
            }
            else if (duration == -1)
            {
                existingEffect.Duration = -1;
                Debug.Log($"Effect {effectName} set to permanent.");
            }
            return;
        }

        effect.Apply(this);
        activeEffects.Add(effect);
        if(effect.Icon != null)
            effectController.InitEffect(activeEffects.Count, effect);

        Debug.Log($"Applied new effect {effectName} with value {value} and duration {duration}.");
    }

    public IBuffEffect GetActiveEffect(string effectName)
    {
        return activeEffects.Find(effect => effect.Name.ToLower() == effectName.ToLower());
    }

    public void RemoveBuffEffect(IBuffEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            effect.Remove(this);
            activeEffects.Remove(effect);
        }
    }

    public void AddItem()
    {
        addedItems.Clear();
        foreach (ItemInventory totalItem in GamePlayController.Instance.PlayerController.TotalInventory.Items)
        {
            int currentQuantity = 0;
            foreach (var playerItem in inventory.Items)
            {
                if (playerItem.itemId == totalItem.itemId && playerItem.isUpgraded == totalItem.isUpgraded)
                {
                    currentQuantity = playerItem.quantity;
                    break;
                }
            }

            int quantityToAdd = (int)Math.Ceiling((totalItem.quantity - currentQuantity) / 2.0);
            if (quantityToAdd > 0)
            {
                inventory.AddItem(totalItem.itemId, quantityToAdd, totalItem.quantity, totalItem.isUpgraded);
                addedItems.Add(new ItemInventory(totalItem.itemId, quantityToAdd, totalItem.isUpgraded));
            }
        }
    }

    public void RemoveItem(string itemId, bool isUpgraded = false)
    {
        inventory.RemoveItem(itemId, 1, isUpgraded);
    }

    public void ClearAllEffects()
    {
        foreach (IBuffEffect effect in activeEffects.ToArray())
        {
            RemoveBuffEffect(effect);
        }
    }

    public void ReceiveDamage(int damage)
    {
        ObserverManager<EventID>.PostEven(EventID.OnTakeDamage, this);
        if (IsDodge)
        {
            Debug.Log("Player dodged the attack! No damage taken.");
            IsDodge = false;
            return;
        }

        if (IsCounterAttack)
        {
            Debug.Log("Player counter-attacked! No damage taken.");
            IsCounterAttack = false;
            return;
        }

        float effectiveDamage = damage - stats.Shield;
        stats.ChangeShield(-damage);
        effectiveDamage = Mathf.Max(0, effectiveDamage);
        stats.ChangeCurHP(-effectiveDamage + (int)Math.Ceiling(damage * stats.DamageAbsorb));
        ObserverManager<IDStateAnimationPlayer>.PostEven(IDStateAnimationPlayer.Hit, null);
        UIDamageController.Instance.ShowDamageText((int)effectiveDamage, this);
        UpdateHpUI();

        if (stats.CurrentHP <= 0)
        {
            EndGame();
        }
    }

    public void UpdateHpUI()
    {
        health.UpdateHp(this);
    }

    public void UpdateArmorUI()
    {
        health.UpdateArmor(this);
    }

    public void DealDamage(int damage)
    {
        ObserverManager<EventID>.PostEven(EventID.OnDealDamage, damage);
    }

    public void ChangeGold(float goldAmount)
    {
        ObserverManager<EventID>.PostEven(EventID.OnGoldChanged, goldAmount);
    }

    public void EndGame()
    {
        ObserverManager<EventID>.PostEven(EventID.OnEndRound);
        DOVirtual.DelayedCall(0.5f, () => {
            ClearAllEffects();
            effectController.ClearAllEffectUI();
            addedItems.Clear();
            inventory.ClearInventory();
            stats.ResetStatAfterRound();
            GamePlayController.Instance.PlayerController.CurPlayerStat = stats;
        });
        this.Health.UnShowHealthBarEnemy();
        PoolingManager.Despawn(gameObject);
        Destroy(transform.parent.gameObject);
    }
}