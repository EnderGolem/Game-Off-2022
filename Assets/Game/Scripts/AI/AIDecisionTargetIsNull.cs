using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionTargetIsNull : AIDecision
{
    public override bool Decide()
    {
        return _brain.Target == null;
    }
}
