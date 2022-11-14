using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionMoveToTarget : AIAction
{
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
        var direction = _brain.Target.transform.position - transform.position;
        direction.y = 0;
        _abilityMove.ProcessInput(direction.normalized);
    }
}
