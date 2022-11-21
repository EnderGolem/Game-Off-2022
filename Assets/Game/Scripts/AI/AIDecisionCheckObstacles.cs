using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionCheckObstacles : AIDecision
{     
    [Tooltip("the distance to compare with")]
    public float Distance  = 1000;
    
    [Tooltip("the layer(s) to look for obstacles on")]
    public LayerMask ObstacleMask ;
    public override bool Decide()
    {
        return ChecObstacles();
    }
    
    ///Возвращает true если есть препятствия
    protected virtual bool ChecObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,(_brain.Target.transform.position - transform.position).normalized , Distance, ObstacleMask);
        return (bool)hit;
    }
}
