using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MeleeAttack : CharacterAbility
{
    [Header("Zone")]
    [SerializeField]
    protected Collider2D damageZone;
    [Header("Timing")]
    [SerializeField]
    protected float minAttackPreparingTime;
    [SerializeField]
    protected float maxAttackPreparingTime;

    [SerializeField] 
    protected float delayBeforeAttacking;
    [SerializeField]
    protected float activeDamageZoneTime;
    [SerializeField] 
    protected float delayAfterAttack;
    [SerializeField]
    protected float timeBetweenAttacks;
    [Header("Properties")]
    [SerializeField] 
    protected float baseDamage;
    [SerializeField] 
    protected float baseDamageToEThroughShield;
    [Tooltip("Выносливость тратимая при ударе")]
    [SerializeField]
    protected float enduranceCost;

    [SerializeField] 
    protected EffectDescription[] attackEffects;
    [SerializeField]
    protected EffectDescription[] throughShieldEffects;

    [Header("Animations")] 
    protected string AttackPreparingParameter = "AttackPreparing";
    protected string AttackingParameter = "Attacking";

    protected string AttackSpeedAnimParameter = "AttackSpeed";
    protected string AttackPreparingSpeedAnimParameter = "AttackPreparingSpeed";
    
    [Header("Feedbacks")]
    [SerializeField]
    protected MMFeedbacks startAttackFeedback;

    protected EffectOnTouch damageZoneOnTouch;
    
    protected PropertyManager _propertyManager;
    protected ObjectProperty damageProperty;
    protected ObjectProperty enduranceProperty;
    protected ObjectProperty damageToEThroughShieldProperty;
    
    protected bool curInput;

    protected float lastAttackStart;

    protected override void PreInitialize()
    {
        base.PreInitialize();
        damageProperty=owner.PropertyManager.AddProperty("AttackDamage", baseDamage);
        enduranceProperty = owner.PropertyManager.GetPropertyByName("Endurance");
        damageToEThroughShieldProperty =
            owner.PropertyManager.AddProperty("MeleeDamageToEThroughShield", baseDamageToEThroughShield);
        damageZoneOnTouch = damageZone.GetComponent<EffectOnTouch>();
        damageZone.enabled = false;
        damageZoneOnTouch.enabled = false;
    }

    private void Update()
    {
        if (curInput && CanAttack())
        {
            StartAttack();
        }

        if (owner.AttackingState.CurrentState == CharacterAttackingState.AttackPreparing)
        {
            HandleAttackPreparing();
        }

        if (owner.AttackingState.CurrentState == CharacterAttackingState.Attacking)
        {
            HandleAttacking();
        }
    }

    protected IEnumerator Attack()
    {
        owner.AttackingState.ChangeState(CharacterAttackingState.AttackPreparing);
        
        yield return new WaitForSeconds(minAttackPreparingTime);

        owner.AttackingState.ChangeState(CharacterAttackingState.Attacking);
        
        for (int i = 0; i < attackEffects.Length; i++)
        {
            damageZoneOnTouch.AddEffect(new Effect(attackEffects[i],owner.PropertyManager));
        }
        
        damageZone.enabled = true;
        damageZoneOnTouch.enabled = true;
        
        yield return new WaitForSeconds(activeDamageZoneTime);

        damageZone.enabled = false;
        damageZoneOnTouch.enabled = false;
        
        yield return new WaitForSeconds(delayAfterAttack);
        
        owner.AttackingState.ChangeState(CharacterAttackingState.Idle);

    }

    protected void StartAttack()
    {
        owner.AttackingState.ChangeState(CharacterAttackingState.AttackPreparing);
        lastAttackStart = Time.time;
    }

    protected void HandleAttackPreparing()
    {
        var delta = Time.time - lastAttackStart;
        if (delta > maxAttackPreparingTime || (delta > minAttackPreparingTime && !curInput))
        {
            for (int i = 0; i < attackEffects.Length; i++)
            {
                damageZoneOnTouch.AddEffect(new Effect(attackEffects[i],owner.PropertyManager));
            }
            for (int i = 0; i < throughShieldEffects.Length; i++)
            {
                damageZoneOnTouch.AddThroughShieldEffect(new Effect(throughShieldEffects[i],owner.PropertyManager));
            }
            
            owner.AttackingState.ChangeState(CharacterAttackingState.Attacking);
            startAttackFeedback?.PlayFeedbacks();
            enduranceProperty?.ChangeCurValue(-enduranceCost);
            lastAttackStart = Time.time;
        }
    }

    protected void HandleAttacking()
    {
        var delta = Time.time - lastAttackStart;

        if (delta > activeDamageZoneTime + delayBeforeAttacking + delayAfterAttack)
        {
            owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
            
            lastAttackStart = Time.time;
        }
        else if (delta > activeDamageZoneTime + delayBeforeAttacking)
        {
            damageZone.enabled = false;
            damageZoneOnTouch.enabled = false;

        }
        else if(delta > delayBeforeAttacking)
        {
            damageZone.enabled = true;
            damageZoneOnTouch.enabled = true;
        }
    }
    

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        owner.Animator.SetBool(AttackPreparingParameter, owner.AttackingState.CurrentState == CharacterAttackingState.AttackPreparing);
        owner.Animator.SetBool(AttackingParameter, owner.AttackingState.CurrentState == CharacterAttackingState.Attacking);
        
        owner.Animator.SetFloat(AttackSpeedAnimParameter,1/(activeDamageZoneTime+delayBeforeAttacking+delayAfterAttack));
        owner.Animator.SetFloat(AttackPreparingSpeedAnimParameter,1/maxAttackPreparingTime);
        
    }

    public void ProcessInput(bool input)
    {
        curInput = input;
    }

    protected bool isReloaded()
    {
        return Time.time - lastAttackStart > timeBetweenAttacks;
    }

    protected bool CanAttack()
    {
        return isReloaded() && !owner.IsTired && owner.AttackingState.CurrentState == CharacterAttackingState.Idle && AbilityAuthorized;
    }
}
