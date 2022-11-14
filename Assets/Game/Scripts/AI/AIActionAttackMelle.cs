using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionAttackMelle : AIAction
{
    protected MeleeAttack _meleeAttack;


    public override void Initialization()
    {
        _meleeAttack = GetComponent<MeleeAttack>();
    }
    public override void PerformAction()
    {
        Attack();
    }

    protected virtual void Attack()
    {
        _meleeAttack.ProcessInput(true);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        _meleeAttack.ProcessInput(false);
    }
}
