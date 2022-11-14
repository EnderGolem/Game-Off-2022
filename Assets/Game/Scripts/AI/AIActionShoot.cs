using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionShoot : AIAction
{
    protected RangedAttack _rangedAttack;


    public override void Initialization()
    {
        _rangedAttack = GetComponent<RangedAttack>();
    }
    public override void PerformAction()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        _rangedAttack.SetTargetPos(_brain.Target.position);
        _rangedAttack.ProcessInput(true);
    }
    public override void OnExitState()
    {
        base.OnExitState();
        _rangedAttack.ProcessInput(false);
    }

}
