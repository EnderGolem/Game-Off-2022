using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionDash : AIAction
{
    protected AbilityDash _AbilityDash;


    public override void Initialization()
    {
        _AbilityDash = GetComponent<AbilityDash>();
    }
    public override void PerformAction()
    {
        Dash();
    }

    protected virtual void Dash()
    {
        _AbilityDash.ProcessInput(true);
        _AbilityDash.SetMoveInput(_brain.Target.position - transform.position);
        //_AbilityDash.Owner.SetMoveInput(_brain.Target.position - transform.position);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        _AbilityDash.ProcessInput(false);
    }
}