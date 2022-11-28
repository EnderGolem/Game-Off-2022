using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionTimeBetweenAction : AIDecision
{
    [SerializeField]
    protected float lastTimeActive = 0;

    [Tooltip("Через какое повторое время, Десишион выдаст правду")]
    public float timeBetween;

    [Tooltip("Через какое время, в первый раз используется данная способность.")]
    public float timeBetweenStart;

    private void Start()
    {
        lastTimeActive = -timeBetweenStart;
    }

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
