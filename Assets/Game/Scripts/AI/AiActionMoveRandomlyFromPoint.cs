using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AiActionMoveRandomlyFromPoint : AIAction
{
    [SerializeField]
    protected Vector2 startPos;
    [SerializeField]
    protected bool isLeft = false;
    [SerializeField]
    protected float length;
    protected AbilityMove _abilityMove;
    public override void Initialization()
    {
        startPos = transform.position;
        _abilityMove = GetComponent<AbilityMove>();
    }
    public override void PerformAction()
    {
        Move();
    }

    protected virtual void Move()
    {
        var direction = startPos;
        if (isLeft)
            direction.x -= length / 2;
        else
            direction.x += length / 2;
        direction.y = 0;
        direction.x -= transform.position.x;
        if (transform.position.x > startPos.x + length / 2)
            isLeft = true;
        if (transform.position.x < startPos.x - length / 2)
            isLeft = false;
        _abilityMove.ProcessInput(direction.normalized);
        _abilityMove.Owner.SetMoveInput(direction.normalized);
    }
    public override void OnExitState()
    {
        base.OnExitState();
        _abilityMove.ProcessInput(Vector2.zero);
    }
}
