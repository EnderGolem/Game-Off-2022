using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class WeaponHandler : CharacterAbility
{
    [Tooltip("Оружие устанавливаемое по умолчанию")]
    [SerializeField]
    protected Weapon defaultWeapon;
    //Список оружия, доступного из инвентаря в данный момент
    protected List<Weapon> availableWeapons;
    [Range(0.01f,1)]
    [SerializeField]
    protected float inputBuffer = 0.4f;
    [MMReadOnly]
    protected Weapon currentWeapon;

    protected CharacterAbility[] abilities;

    protected int curWeaponNum = 0;

    protected float curScroll;

    protected float lastInputScrollTime;
    protected override void PreInitialize()
    {
        base.PreInitialize();
        availableWeapons = new List<Weapon>();
        abilities = GetComponentsInParent<CharacterAbility>();
        lastInputScrollTime = -10000;
    }

    protected override void Initialize()
    {
        base.Initialize();
        ChooseCurrentWeapon();
    }

    protected void Update()
    {
        if (CanChangeWeapon() && Time.time - lastInputScrollTime < inputBuffer)
        {
            lastInputScrollTime = 0;
            
            curWeaponNum += (int)Mathf.Sign(curScroll);
            if (curWeaponNum >= availableWeapons.Count)
            {
                curWeaponNum = 0;
            }
            else if(curWeaponNum < 0)
            {
                curWeaponNum = availableWeapons.Count - 1;
            }
            
            ChooseCurrentWeapon();
        }
    }

    protected void ChooseCurrentWeapon()
    {
        if (availableWeapons.Count == 0 || availableWeapons[curWeaponNum] == null)
        {
            SwapWeapon(defaultWeapon);
        }
        else
        {
            SwapWeapon(availableWeapons[curWeaponNum]);
        }
    }

    protected void SwapWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {

            for (int i = 0; i < currentWeapon.ActiveAbilities.Length; i++)
            {
                var t = abilities.Where((CharacterAbility ab) => ab.AbilityName == currentWeapon.ActiveAbilities[i]);
                foreach (var a in t)
                {
                    a.abilityPermitted = false;
                }
            }
        }

        currentWeapon = newWeapon;
        
        if (currentWeapon != null)
        {
            if (currentWeapon.AnimatorController != null)
            {
                owner.Animator.runtimeAnimatorController = currentWeapon.AnimatorController;
            }
            for (int i = 0; i < currentWeapon.ActiveAbilities.Length; i++)
            {
                var t = abilities.Where((CharacterAbility ab) => ab.AbilityName == currentWeapon.ActiveAbilities[i]);
                foreach (var a in t)
                {
                    a.abilityPermitted = true;
                }
            }
        }
    }

    public void ProcessInput(float input)
    {
        if (input != 0)
        {
            curScroll = input;
            lastInputScrollTime = Time.time;
        }
    }

    public void UpdateAvailableWeapons(List<Weapon> newAvailableWeapons)
    {
        availableWeapons = newAvailableWeapons;
        
        ChooseCurrentWeapon();
    }

    protected bool CanChangeWeapon()
    {
        return AbilityAuthorized && owner.AttackingState.CurrentState == CharacterAttackingState.Idle;
    }

}
