using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : CharacterAbility
{
    [SerializeField]
    protected Shield shield;
    [Tooltip("Модификатор силы отталкивания во время блокирования")]
    [SerializeField]
    protected float knockbackModifier;
    [Tooltip("Список эффектов, накладываемых на игрока, когда он использует щит" +
             "Используется например для замедления игрока")]
    [SerializeField]
    protected EffectDescription[] effectsInBlock;
    [SerializeField]
    protected string blockAnimParameter = "Blocking";
    /// <summary>
    /// Последнее направление ввода
    /// </summary>
    protected bool curInput = false;

    protected override void PreInitialize()
    {
        base.PreInitialize();
        shield.Initialize(owner,GetComponent<Collider2D>(),knockbackModifier);
    }

    private void Update()
    {
        if (CanBlock() && curInput)
        {
            if(owner.AttackingState.CurrentState != CharacterAttackingState.Blocking)
            StartBlocking();
        }
        else
        {
            if(owner.AttackingState.CurrentState == CharacterAttackingState.Blocking)
            StopBlocking();
        }
        
    }

    protected void StartBlocking()
    {
        owner.AttackingState.ChangeState(CharacterAttackingState.Blocking);
        
        for (int i = 0; i < effectsInBlock.Length; i++)
        {
            owner.PropertyManager.AddEffect(new Effect(effectsInBlock[i]));   
        }
        shield.Activate();
    }

    protected void StopBlocking()
    {
        owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
        
        for (int i = 0; i < effectsInBlock.Length; i++)
        {
            owner.PropertyManager.RemoveEffect(effectsInBlock[i].Id);   
        }
        shield.Deactivate();
    }

    public void ProcessInput(bool input)
    {
        curInput = input;
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        owner.Animator.SetBool(blockAnimParameter,owner.AttackingState.CurrentState == CharacterAttackingState.Blocking);
    }

    protected bool CanBlock()
    {
        return AbilityAuthorized && !owner.IsTired  
               && (owner.AttackingState.CurrentState == CharacterAttackingState.Idle 
                   || owner.AttackingState.CurrentState == CharacterAttackingState.Blocking);
    }
}
