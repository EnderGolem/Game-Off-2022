using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionTurnToTarget : AIAction
{
    protected Character owner;
    public bool defaultRight;
    public override void Initialization()
    {
        base.Initialization();
        owner = GetComponent<Character>();
    }

    public override void PerformAction()
    {
        if (_brain.Target != null)
        {
            owner.SetMoveInput(_brain.Target.transform.position - transform.position);
        }
        else
        {
            if (defaultRight)
            {
                owner.SetMoveInput(new Vector2(1,0));
            }
            else
            {
                owner.SetMoveInput(new Vector2(-1,0));
            }
        }
    }
}
