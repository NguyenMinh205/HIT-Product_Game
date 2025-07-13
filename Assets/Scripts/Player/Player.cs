using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float distancePlayerAndHealthBar;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private HealthBar health;

    private Character character;
    private CharacterStatSO stats;
    public CharacterStatSO Stats => stats;
    private ICharacterAbility ability;

    private List<IBuffEffect> activeEffects = new List<IBuffEffect>();
    [SerializeField] private Inventory inventory;
    private List<ItemInventory> addedItems = new List<ItemInventory>(); // Danh sách vật phẩm được thêm bởi AddItem

    public bool IsDodge { get; set; }
    public bool IsCounterAttack { get; set; }

    public Inventory Inventory => inventory;
    public List<ItemInventory> AddedItems => addedItems; // Getter cho danh sách vật phẩm đã thêm

    public HealthBar Health
    {
        get => health;
        set => health = value;
    }

    public void Initialize(Character selectedCharacter, CharacterStatSO characterStatSO, int index)
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

        ability = CharacterAbilityFactory.CreateAbility(character.id);
        if (ability != null && stats != null)
        {
            ability.StartSetup(this);
        }

        if (inventory != null)
        {
            foreach (ItemInventory item in GamePlayController.Instance.PlayerController.TotalInventory.Items)
            {
                inventory.AddItem(item.itemBase, (int)Math.Ceiling(item.quantity / 2.0), item.quantity);
            }
        }
        UIHealthBarController.Instance.InitHealthBarToObjectBase(this);
        health.UpdateHp(this);
        health.UpdateArmor(this);
    }

    public void CalculationPositionPlayer(Vector3 posPlayer)
    {
        playerSprite = GetComponent<SpriteRenderer>();
        float height = playerSprite.bounds.extents.y;
        Vector3 newPos = posPlayer + Vector3.up * height + Vector3.up * distancePlayerAndHealthBar;
        transform.position = newPos;
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
                if (playerItem.itemBase.id == totalItem.itemBase.id)
                {
                    currentQuantity = playerItem.quantity;
                    break;
                }
            }

            int quantityToAdd = (int)Math.Ceiling((totalItem.quantity - currentQuantity) / 2.0);
            if (quantityToAdd > 0)
            {
                inventory.AddItem(totalItem.itemBase, quantityToAdd, totalItem.quantity);
                addedItems.Add(new ItemInventory(totalItem.itemBase, quantityToAdd));
                Debug.Log($"Added item {totalItem.itemBase.id} with quantity {quantityToAdd} to Player inventory");
            }
        }
    }

    public void RemoveItem(ItemBase itemBase)
    {
        foreach (ItemInventory item in inventory.Items)
        {
            if (item.itemBase == itemBase)
            {
                inventory.RemoveItem(itemBase, 1);
                break;
            }
        }
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
        ObserverManager<EventID>.PostEven(EventID.OnTakeDamage, damage);
        if (IsDodge)
        {
            IsDodge = false;
            return;
        }

        if (IsCounterAttack)
        {
            IsCounterAttack = false;
            return;
        }

        float effectiveDamage = damage - stats.Shield;
        stats.ChangeShield(-damage);
        effectiveDamage = Mathf.Max(0, effectiveDamage);
        stats.ChangeCurHP(-effectiveDamage);

        health.UpdateArmor(this);
        health.UpdateHp(this);

        if (stats.CurrentHP <= 0)
        {
            EndGame();
        }
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
        ClearAllEffects();
        addedItems.Clear();
        inventory.ClearInventory();
        Debug.Log("Game Over for Player");
        this.Health.UnShowHealthBarEnemy();
        PoolingManager.Despawn(gameObject);
    }
}