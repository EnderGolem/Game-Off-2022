using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AIDecisionCanGoInDirection : AIDecision
{
    [Tooltip("Смещений проверки препятствий относительно персонажа")]
    [SerializeField]
    protected Vector2 obstacleCheckOffset;
    [Tooltip("Размер проверки земли")]
    [SerializeField]
    protected Vector2 obstacleCheckSize;
    [SerializeField] 
    protected LayerMask obstacleMask;
    
    [Tooltip("Ссылка на трансформ проверки персонажа на соприкосновение с землей")]
    [SerializeField]
    protected Transform groundCheckPoint;
    [Tooltip("Смещений проверки земли относительно персонажа")]
    [SerializeField]
    protected Vector2 groundCheckOffset;
    [Tooltip("Размер проверки земли")]
    [SerializeField]
    protected Vector2 groundCheckSize;
    [SerializeField] 
    protected LayerMask groundMask;
    [SerializeField]
    protected DirDetermineType _dirDetermineType;

    protected Rigidbody2D body;
    public override void Initialization()
    {
        base.Initialization();
        body = GetComponent<Rigidbody2D>();
    }

    public override bool Decide()
    {
        var dir = DetermineDir();

        var obstCheckPos = (Vector2) transform.position + obstacleCheckOffset * dir;
        var hit = Physics2D.OverlapBox(obstCheckPos, obstacleCheckSize, 0,obstacleMask);
        if (hit) return false;
        
        var grCheckPos = (Vector2)groundCheckPoint.position + groundCheckOffset * dir; 
        hit = Physics2D.OverlapBox(grCheckPos, groundCheckSize, 0,groundMask);
        if (!hit) return false;
        
        return true;
    }

    float DetermineDir()
    {
        if (_dirDetermineType == DirDetermineType.ToCurrentMoveDir
        || ((_dirDetermineType==DirDetermineType.ToTarget 
             || _dirDetermineType == DirDetermineType.FromTarget)&& _brain.Target == null))
        {
            return Mathf.Sign(body.velocity.x);
        }

        if (_dirDetermineType == DirDetermineType.Left)
        {
            return -1;
        }
        if(_dirDetermineType == DirDetermineType.Right)
        {
            return 1;
        }

        if (_dirDetermineType == DirDetermineType.ToTarget)
        {
            return Mathf.Sign(_brain.Target.position.x - transform.position.x);
        }
        
        if (_dirDetermineType == DirDetermineType.FromTarget)
        {
            return -Mathf.Sign(_brain.Target.position.x - transform.position.x);
        }

        return 0;
    }
    

    private void OnDrawGizmos()
    {
        MMDebug.DrawRectangle((Vector2)groundCheckPoint.position+groundCheckOffset,Color.magenta, groundCheckSize);
        MMDebug.DrawRectangle((Vector2)transform.position + obstacleCheckOffset,Color.blue, obstacleCheckSize);
    }
}

public enum DirDetermineType
{
    Left,Right,ToTarget,FromTarget,ToCurrentMoveDir
}
