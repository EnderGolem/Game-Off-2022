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
    private void Awake()
    {
        _character = GetComponent<Character>();
        _abilityMove = GetComponent<AbilityMove>();
        _abilityJump = GetComponent<AbilityJump>();
        _abilityDash = GetComponent<AbilityDash>();
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
    
    
}
