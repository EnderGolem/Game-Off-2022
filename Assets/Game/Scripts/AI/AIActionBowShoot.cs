using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionBowShoot : AIAction
{ 
    [SerializeField]
    protected RangedAttack bowAttack;
    [SerializeField]
    protected float aimTime;
    [SerializeField]
    protected LayerMask obstacleMask;

    protected float lastAttackTime = -1000;

    protected Character owner;

    protected bool isAiming;
    public override void Initialization()
    {
        base.Initialization();
        owner = GetComponent<Character>();
    }

    public override void PerformAction()
    {
        if (owner.AttackingState.CurrentState == CharacterAttackingState.Idle)
        {
            StartAttack();
        }
        else if (isAiming)
        {
            Aiming();
            if (Time.time - lastAttackTime > aimTime)
            {
                isAiming = false;
                Shot();
            }
        }
    }

    protected void StartAttack()
    {
        lastAttackTime = Time.time;
        if (!_brain.Target)
        {
            Debug.Log(gameObject.name + " Нет цели у мозга ИИ" );
            return;
        }

        bowAttack.SetTargetPos(_brain.Target.position);
        bowAttack.ProcessInput(true);
        isAiming = true;
    }

    protected void Aiming()
    {
        bowAttack.SetTargetPos(_brain.Target.position);
        var dist = _brain.Target.position - bowAttack.ProjStartPosition.position;
        var hit = Physics2D.Raycast(bowAttack.ProjStartPosition.position,
            _brain.Target.position - bowAttack.ProjStartPosition.position, dist.magnitude, obstacleMask);
        if (hit)
        {
            bowAttack.SetUseDirectFire(false);
        }
        else
        {
            bowAttack.SetUseDirectFire(true);
        }
    }

    protected void Shot()
    {
        bowAttack.ProcessInput(false);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        bowAttack.ProcessInput(false);
    }
}
