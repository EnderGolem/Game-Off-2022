using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionTurnToTarget : AIAction
{
    protected Character owner;

    public override void Initialization()
    {
        base.Initialization();
        owner = GetComponent<Character>();
    }

    public override void PerformAction()
    {
        owner.SetMoveInput(_brain.Target.transform.position - transform.position); 
    }
}
