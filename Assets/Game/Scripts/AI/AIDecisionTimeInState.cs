using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionTimeInState : AIDecision
{
    [Tooltip("Если время в состоянии больше этого, то автоматически срабатывает переход")]
    [SerializeField]
    protected float maxTimeInState;
    public override bool Decide()
    {
        return _brain.TimeInThisState >= maxTimeInState;
    }
}
