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
    private AbilityMove _abilityMove;
    private AbilityJump _abilityJump;
    private void Awake()
    {
        _abilityMove = GetComponent<AbilityMove>();
        _abilityJump = GetComponent<AbilityJump>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_abilityMove != null)
        {
            _abilityMove.ProcessInput(context.ReadValue<Vector2>());
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
}
