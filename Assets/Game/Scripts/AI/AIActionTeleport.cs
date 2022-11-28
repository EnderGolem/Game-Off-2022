using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionTeleport : AIAction
{
    [Tooltip("Смещение относительно Таргета")]
    public Vector2 offsetRelativeTarget;

    [Tooltip("Переместить персонажа зеркально относительно цели?")]
    public bool targetMirror;

    protected Character _character;

    
    public override void PerformAction()
    {
        Teleport();
    }

    protected virtual void Teleport()
    {
        if (!_brain.Target)
        {
            Debug.Log(gameObject.name + "не имеет цели в мозге");
            return;
        }
        _character = _brain.Target.GetComponent<Character>();
        var newPosition = (Vector2)_brain.Target.position;
        int direction = 1;
        if(_character == null)
            return;
        if (!_character.IsFacingRight)
            direction *= -1;
        if(targetMirror)
            direction *= -1;
        
        if(direction == 1)
             newPosition += offsetRelativeTarget;
        else
        {
            newPosition.x -= offsetRelativeTarget.x;
            newPosition.y += offsetRelativeTarget.y;
        }

        transform.position = newPosition;

    }
}
