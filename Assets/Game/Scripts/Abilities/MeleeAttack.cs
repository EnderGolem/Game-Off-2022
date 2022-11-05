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
    
    protected bool curInput;

    protected override void PreInitialize()
    {
        base.PreInitialize();
        damageZone.enabled = false;
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
        
        damageZone.enabled = true;
        
        yield return new WaitForSeconds(activeDamageZoneTime);

        damageZone.enabled = false;
        
        yield return new WaitForSeconds(delayAfterAttack);
        
        owner.AttackingState.ChangeState(CharacterAttackingState.Idle);

    }

    public override void ProcessInput(bool input)
    {
        base.ProcessInput(input);
        curInput = input;
    }
}
