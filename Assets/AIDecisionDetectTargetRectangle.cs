using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionDetectTargetRectangle : AIDecision
{

    [Tooltip("Высота прямоугольника")]
    public float height;
    [Tooltip("Длина прямоугольника")]
    public float length;
   
    [Tooltip("Замечает ли обьекты снизу")]
    public bool DownDetection = false;
    [Tooltip("Центр прямоугольника")]
    public Vector3 DetectionOriginOffset = new Vector3(0, 0, 0);
    [Tooltip("Слой на котором ищем")]
    public LayerMask TargetLayer;
    [Tooltip("Следует ли захватывать цель при обнаружении" +
             "Если нет то можно использовать для простой проверки препятствий")]
    public bool fixTarget=true;
    
    [Tooltip("the layer(s) to look for obstacles on")]
    public LayerMask ObstacleMask ;
    protected Collider2D _collider;
    protected Character _character;
    protected bool _init = false;
    protected Collider2D _detectionCollider = null;
    protected Vector2 _raycastOrigin;
    protected Color _gizmoColor = Color.yellow;
    protected Vector2 _boxcastDirection;
    

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
        _raycastOrigin = transform.position +  ((_character.IsFacingRight)?DetectionOriginOffset:-DetectionOriginOffset);
         
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

        if (DownDetection)
        {
            if(fixTarget)
            _brain.Target = _detectionCollider.gameObject.transform;
            return true;
        }
        else
        {
            //FIX странный баг, время от времени, он может не детектить, почему, понять не могу, причем он либо работает за запуск, либо нет.
            _boxcastDirection = (Vector2)(_detectionCollider.gameObject.MMGetComponentNoAlloc<Collider2D>().bounds.center - _collider.bounds.center);
            RaycastHit2D hit = Physics2D.Linecast(_collider.bounds.center,_detectionCollider.transform.position, ObstacleMask);
         
            if (!hit)
            {   
                if(fixTarget)
                _brain.Target = _detectionCollider.gameObject.transform;
                return true;
            }
            else
            {  
                return false;
            }             
        }
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
        Gizmos.DrawWireCube(transform.position+DetectionOriginOffset, new Vector3(length,height));
        /*if (_init)
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(_raycastOrigin, new Vector3(length,height));
        }*/            
    }
}