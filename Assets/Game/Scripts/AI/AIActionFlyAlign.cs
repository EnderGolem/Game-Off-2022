using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionFlyAlign : AIAction
{
    protected AbilityFly _abilityFly;
    [SerializeField]
    protected Vector2 flyDir;

    protected Character owner;
    public override void Initialization()
    {
        base.Initialization();
        owner = GetComponent<Character>();
        _abilityFly = GetComponent<AbilityFly>();
    }

    public override void PerformAction()
    {
        var dir = (owner.IsFacingRight) ? flyDir : -flyDir;
        _abilityFly.ProcessInput(dir.normalized);
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        _abilityFly.StartFly();
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _abilityFly.StopFly();
    }
}
