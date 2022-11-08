using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Компонент устанавливающий связь между вводом и применением способностей персонажа
/// </summary>
public class CharacterControl : MonoBehaviour
{
    private Character _character;
    private AbilityMove _abilityMove;
    private AbilityJump _abilityJump;
    private AbilityDash _abilityDash;
    private AbilityGetDown _abilityGetDown;
    private MeleeAttack _meleeAttack;
    private RangedAttack _rangedAttack;
    private WeaponHandler _weaponHandler;
    private void Awake()
    {
        _character = GetComponent<Character>();
        _abilityMove = GetComponent<AbilityMove>();
        _abilityJump = GetComponent<AbilityJump>();
        _abilityDash = GetComponent<AbilityDash>();
        _abilityGetDown = GetComponent<AbilityGetDown>();
        _meleeAttack = GetComponent<MeleeAttack>();
        _rangedAttack = GetComponent<RangedAttack>();
        _weaponHandler = GetComponent<WeaponHandler>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_character != null)
        {
            _character.SetMoveInput(context.ReadValue<Vector2>());
        }

        if (_abilityMove != null)
        {
            _abilityMove.ProcessInput(context.ReadValue<Vector2>());
        }

        if (_abilityJump != null)
        {
            _abilityJump.SetMoveInput(context.ReadValue<Vector2>());
        }
        if (_abilityDash != null)
        {
            _abilityDash.SetMoveInput(context.ReadValue<Vector2>());
        }

        if (_abilityGetDown != null)
        {
            _abilityGetDown.SetMoveInput(context.ReadValue<Vector2>());
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_abilityJump != null)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _abilityJump.ProcessInput(true);
            }
            else if(context.phase == InputActionPhase.Canceled)
            {
                _abilityJump.ProcessInput(false);
            }
        }
    }
    
    public void Dash(InputAction.CallbackContext context)
    {
        if (_abilityDash != null)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _abilityDash.ProcessInput(true);
            }
            else if(context.phase == InputActionPhase.Canceled)
            {
                _abilityDash.ProcessInput(false);
            }
        }
    }
    public void GetDown(InputAction.CallbackContext context)
    {
        if (_abilityGetDown != null)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _abilityGetDown.ProcessInput(true);
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                _abilityGetDown.ProcessInput(false);
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (_meleeAttack != null)
        {
            if (context.started)
            {
                _meleeAttack.ProcessInput(true);
            }
            else if (context.canceled)
            {
                _meleeAttack.ProcessInput(false);
            }
        }
    }

    public void SecondaryAttack(InputAction.CallbackContext context)
    {
        if (_rangedAttack != null)
        {
            if (context.started)
            {
                _rangedAttack.ProcessInput(true);
            }
            else if (context.canceled)
            {
                _rangedAttack.ProcessInput(false);
            }
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
       
    }

    public void Scroll(InputAction.CallbackContext context)
    {
        if (_weaponHandler != null)
        {
            _weaponHandler.ProcessInput(context.ReadValue<float>());
        }
    }

}
