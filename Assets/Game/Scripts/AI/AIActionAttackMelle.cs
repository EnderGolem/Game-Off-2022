using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIActionAttackMelle : AIAction
{
    [SerializeField]
    protected MeleeAttack _meleeAttack;
    

    public override void Initialization()
    {
        if(_meleeAttack==null)
        _meleeAttack = GetComponent<MeleeAttack>();
    }
    public override void PerformAction()
    {
        Attack();
    }

    protected virtual void Attack()
    {
        _meleeAttack.ProcessInput(true);
        _meleeAttack.Owner.SetMoveInput(_brain.Target.position - transform.position);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        _meleeAttack.ProcessInput(false);
    }
}
