using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedAttack : CharacterAbility
{
    protected bool curInput;
    [Tooltip("Снаряды используемые при стрельбе")]
    [SerializeField]
    protected GameObject projectile;
    [Tooltip("Точка из которой вылетают снаряды")]
    [SerializeField]
    protected Transform projStartPos;
    [Tooltip("Насколько будет повернут снаряд к горизонту при старте, относительно начального поворота")]
    [SerializeField]
    protected float spawnAngleOffset;
    
    [SerializeField]
    protected float delayBeforeAttack;
    [SerializeField] 
    protected float delayAfterAttack;
    [SerializeField]
    protected float baseDamage;
    [SerializeField]
    protected EffectDescription[] attackEffects;
    [Tooltip("Следует ли использовать инвентарь для получения информации о зарядке оружия или делать это самостоятельно")] 
    [SerializeField]
    protected bool UseInventoryToManageAmmo;
    [Tooltip("Имя оружия для обращения к инвентарю")]
    [SerializeField]
    protected string weaponName;
    [Tooltip("Следует ли при выстреле рассчитывать угола выстрела так, чтобы попасть по заданным координатам?"+
    "Полезно для ботов")]
    [SerializeField]
    protected bool UseBalliscticToTarget;
    [Tooltip("Следует ли рассчитывать баллистику по координатам курсора, если персонажем управляет игрок")]
    [SerializeField]
    protected bool UseCursorToAimBallistic;
    /// <summary>
    /// Координаты по которым будет производится выстрел
    /// </summary>
    protected Vector2 curTargetPos;

    protected ObjectProperty damageProperty;
    /// <summary>
    /// Следует ли при рассчете баллистической траектори ориентироваться на фазу подъема или на фазу спуска
    /// </summary>
    protected bool useDirectFire = true;

    protected InventoryHandler _inventoryHandler;
    
    protected override void PreInitialize()
    {
        base.PreInitialize();
        _inventoryHandler = GetComponent<InventoryHandler>();
        damageProperty=owner.PropertyManager.AddProperty("RangedAttackDamage", baseDamage);
    }
    private void Update()
    {
        if (curInput && CanAttack())
        {
            StartCoroutine(Attack());
        }
    }

    protected IEnumerator Attack()
    {
        owner.AttackingState.ChangeState(CharacterAttackingState.RangeAttacking);
        
        yield return new WaitForSeconds(delayBeforeAttack);

        if (UseInventoryToManageAmmo) _inventoryHandler.Shoot(weaponName);
        
        SpawnProjectile();
        
        yield return new WaitForSeconds(delayAfterAttack);
        
        owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
    }

    protected void SpawnProjectile()
    {
        ///Добавляем offset если не надо рассчитывать точный угол
        Quaternion rot = (UseBalliscticToTarget)
            ? projStartPos.rotation
            : Quaternion.Euler(projStartPos.rotation.eulerAngles + new Vector3(0, 0, spawnAngleOffset));
        var proj = Instantiate(projectile, projStartPos.position, 
            rot)
            .GetComponent<Projectile>();
        if (UseBalliscticToTarget)
        {
            if (UseCursorToAimBallistic)
            {
                SetTargetPos(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            }
            
            proj.SetAngleToPosition(curTargetPos,useDirectFire);
        }

        var eff = proj.GetComponent<EffectOnTouch>();
        eff.SetOwner(owner);
        for (int i = 0; i < attackEffects.Length; i++)
        {
            eff.AddEffect(new Effect(attackEffects[i],owner.PropertyManager));
        }
        
        proj.enabled = true;
    }

    protected bool CanAttack()
    {
        return IsLoaded() && owner.AttackingState.CurrentState == CharacterAttackingState.Idle && AbilityAuthorized;
    }

    protected bool IsLoaded()
    {
        if (UseInventoryToManageAmmo)
        {
            return _inventoryHandler.CanShoot(weaponName);
        }

        return true;
    }

    public void ProcessInput(bool input)
    {
        curInput = input;
    }
    /// <summary>
    /// Ввод данных позиции, куда надо стрелять 
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetPos(Vector2 target)
    {
        curTargetPos = target;
    }

    public void SetUseDirectFire(bool flag)
    {
        useDirectFire = flag;
    }

    private void OnDrawGizmos()
    {
        if (UseBalliscticToTarget)
        {
            var p = projectile.GetComponent<Projectile>();
            var f = p.CalculateAngleToHitDesignatedPosition(
                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), projStartPos.position,
                out float angle,useDirectFire);
            if (f)
            {
                Vector2 curPos = projStartPos.position;
                Vector2 curVelocity;
                if (projStartPos.right.x >= 0)
                {
                    curVelocity = new Vector2(p.InitialVelocity, 0).MMRotate(angle);
                }
                else
                {
                    curVelocity = new Vector2(p.InitialVelocity, 0).MMRotate(180 - angle);
                }

                float step = 0.04f;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 newPos = curPos + curVelocity * step;
                    curVelocity += new Vector2(0, Physics2D.gravity.y * p.RigidBody.gravityScale * step);
                    Gizmos.DrawLine(curPos, newPos);
                    curPos = newPos;
                }
            }
        }
    }
}
