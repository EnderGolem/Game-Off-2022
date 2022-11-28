using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionTargetIsAlive : AIDecision
{
    public override bool Decide()
    {
        if (_brain.Target == null) return false;
        var health = _brain.Target.GetComponent<Health>();
        if (health == null || !health.IsAlive) return false;

        return true;
    }
}
