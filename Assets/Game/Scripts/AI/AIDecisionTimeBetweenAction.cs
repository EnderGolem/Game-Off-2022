using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionTimeBetweenAction : AIDecision
{
    protected float lastTimeActive = 0;

    [Tooltip("Через какое повторое время, Десишион выдаст правду")]
    public float timeBetween;

    public override bool Decide()
    {
        if (Time.time - lastTimeActive >= timeBetween)
        {
            lastTimeActive = Time.time;
            return true;
        }
        return false;
    }

}
