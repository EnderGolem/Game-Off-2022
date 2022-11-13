using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionDetectTargetRectangle : AIDecision
{

    public float height;
    public float length;
    
    public bool DownDetection = false;
    [Tooltip("the center of the search rectanlge")]
    public Vector3 DetectionOriginOffset = new Vector3(0, 0, 0);
    
    [Tooltip("the layer(s) to search our target on")]
    public LayerMask TargetLayer;
    protected Collider2D _collider;
    protected Character _character;
    protected bool _init = false;
    protected Collider2D _detectionCollider = null;
    protected Vector2 _raycastOrigin;
    protected Color _gizmoColor = Color.yellow;

    /// <summary>
    /// On init we grab our Character component
    /// </summary>
    public override void Initialization()
    {
        _character = this.gameObject.GetComponentInParent<Character>();
        //   _collider = this.gameObject.GetComponentInParent<Collider2D>(); было
        _collider = this.gameObject.GetComponentInParent<Rigidbody2D>().GetComponentInChildren<Collider2D>();
        //TODO _gizmoColor.a = 0.25f;
        _init = true;
    }
    public override bool Decide()
    {
        return DetectTarget();
    }

    protected virtual bool DetectTarget()
    {
        _detectionCollider = null;
        _raycastOrigin = transform.position +  DetectionOriginOffset;

        var pointA = _raycastOrigin;
        pointA.y = (float)(pointA.y - height / 2);
        pointA.x = (float)(pointA.x + length / 2);
        var pointB = _raycastOrigin;
        pointB.y = (float)(pointB.y + height / 2);
        pointB.x = (float)(pointB.x - length / 2);
        _detectionCollider = Physics2D.OverlapArea(pointA, pointB, TargetLayer);

        if (_detectionCollider == null)
        {
            return false;
        }

        if (!DownDetection)
        {
            _brain.Target = _detectionCollider.gameObject.transform;
            return true;
        }
        //else DownDetection, не реализована.
        return false;
    }
    protected virtual void OnDrawGizmosSelected()
    {
       

        var pointA = _raycastOrigin;
        pointA.y = (float)(pointA.y - height / 2);
        pointA.x = (float)(pointA.x + length / 2);
        var pointB = _raycastOrigin;
        pointB.y = (float)(pointB.y + height / 2);
        pointB.x = (float)(pointB.x - length / 2);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_raycastOrigin, new Vector3(length,height));
        if (_init)
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(_raycastOrigin, new Vector3(length,height));
        }            
    }
}