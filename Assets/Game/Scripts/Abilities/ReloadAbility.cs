using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAbility : CharacterAbility
{
   [Tooltip("Время перезарядки")]
   [SerializeField]
   protected float reloadTime;
   [Tooltip("Оружие, которое мы перезаряжаем в инвентаре")]
   [SerializeField]
   protected string weaponName;
   [Tooltip("Нужно ли зажимать клавишу перезарядки на протяжении всего времени перезарядки")]
   [SerializeField]
   protected bool needHoldToUse;

   protected Coroutine curReloadRoutine;

   protected bool curInput;

   protected InventoryHandler _inventoryHandler;

   protected override void PreInitialize()
   {
      base.PreInitialize();
      _inventoryHandler = GetComponent<InventoryHandler>();
   }

   private void Update()
   {
      if (curInput && CanReload())
      {
         curReloadRoutine = StartCoroutine(StartReloading());
      }
   }
   
   public void ProcessInput(bool input)
   {
      curInput = input;
      ///Прерываем перезарядку, если перестали удерживать кнопку перезарядки
      if (!input && needHoldToUse && owner.AttackingState.CurrentState == CharacterAttackingState.Reloading)
      {
         InterruptReload();
      }
   }

   protected IEnumerator StartReloading()
   {
      owner.AttackingState.ChangeState(CharacterAttackingState.Reloading);
      
      yield return new WaitForSeconds(reloadTime);
      
      Reload();
      owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
   }

   protected void InterruptReload()
   {
      StopCoroutine(curReloadRoutine);
      owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
   }

   protected void Reload()
   {
      Debug.Log("Reload");
      _inventoryHandler.ReloadWeapon(weaponName);
   }

   protected bool CanReload()
   {
      return AbilityAuthorized && owner.AttackingState.CurrentState == CharacterAttackingState.Idle
         && _inventoryHandler.CanReload(weaponName);
   }
}
