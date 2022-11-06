using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : CharacterAbility<bool>
{
    [SerializeField]
    protected Collider2D damageZone;
    [SerializeField]
    protected float delayBeforeAttack;
    [SerializeField]
    protected float activeDamageZoneTime;
    [SerializeField] 
    protected float delayAfterAttack;

    [SerializeField] 
    protected float baseDamage;

    [SerializeField] 
    protected EffectDescription[] attackEffects;

    protected EffectOnTouch damageZoneOnTouch;
    
    protected PropertyManager _propertyManager;
    protected ObjectProperty damageProperty;
    
    protected bool curInput;

    protected override void PreInitialize()
    {
        base.PreInitialize();
        damageProperty=owner.PropertyManager.AddProperty("AttackDamage", baseDamage);
        damageZoneOnTouch = damageZone.GetComponent<EffectOnTouch>();
        damageZone.enabled = false;
        damageZoneOnTouch.enabled = false;
    }

    private void Update()
    {
        if (curInput && owner.AttackingState.CurrentState != CharacterAttackingState.Attacking)
        {
            StartCoroutine(Attack());
        }
    }

    protected IEnumerator Attack()
    {
        owner.AttackingState.ChangeState(CharacterAttackingState.Attacking);
        
        yield return new WaitForSeconds(delayBeforeAttack);

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

    public override void ProcessInput(bool input)
    {
        base.ProcessInput(input);
        curInput = input;
    }
}
