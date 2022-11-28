using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionMoveToTarget : AIAction
{
    [Tooltip("Если включить этот флажок, то персонаж будет убегать от цели")]
    [SerializeField]
    protected bool moveOppositeDirection;
    
    protected AbilityMove _abilityMove;

    public override void Initialization()
    {
        _abilityMove = GetComponent<AbilityMove>();
    }


    public override void PerformAction()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (!_brain.Target)
            return;
        
        var direction = _brain.Target.transform.position - transform.position;
        direction.y = 0;
        if (moveOppositeDirection)
        {
            direction = -direction;
        }

        _abilityMove.ProcessInput(direction.normalized);
        _abilityMove.Owner.SetMoveInput(direction.normalized);
    }
    public override void OnExitState()
    {
        base.OnExitState();
        _abilityMove.ProcessInput(Vector2.zero);
    }
}
