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
    
    
    protected Collider2D _collider;
    public override void Initialization()
    {
        _collider = this.gameObject.GetComponentInParent<Rigidbody2D>().GetComponentInChildren<Collider2D>();
    }
    public override bool Decide()
    {
        return ChecObstacles();
    }
    
    ///Возвращает true если есть препятствия
    protected virtual bool ChecObstacles()
    {
        if (_brain.Target == null)
            return false;
       // RaycastHit2D hit = Physics2D.Raycast(_collider.bounds.center,(_brain.Target.transform.position - _collider.bounds.center).normalized , Distance, ObstacleMask);
       RaycastHit2D hit = Physics2D.Linecast(_collider.bounds.center,_brain.Target.transform.position  ,ObstacleMask);
        return (bool)hit;
    }
}
