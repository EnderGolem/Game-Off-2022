using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

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
   [Tooltip("Фидбэк, работающий пока идет перезарядка")]
   [SerializeField]
   protected MMFeedbacks reloadingFeedback;
   [Tooltip("Фидбэк, вызываемый, когда перезарядка завершена")]
   [SerializeField]
   protected MMFeedbacks stopReloadFeedback;
   [SerializeField]
   protected string reloadAnimParam = "Reloading";
   [SerializeField]
   protected string reloadSpeedAnimParam = "ReloadSpeed";
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
      reloadingFeedback?.PlayFeedbacks();
      yield return new WaitForSeconds(reloadTime);
      reloadingFeedback?.StopFeedbacks();
      Reload();
      owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
   }

   protected void InterruptReload()
   {
      reloadingFeedback?.StopFeedbacks();
      StopCoroutine(curReloadRoutine);
      owner.AttackingState.ChangeState(CharacterAttackingState.Idle);
   }

   protected void Reload()
   {
     
      _inventoryHandler.ReloadWeapon(weaponName);
      stopReloadFeedback?.PlayFeedbacks();
   }

   protected bool CanReload()
   {
      return AbilityAuthorized && owner.AttackingState.CurrentState == CharacterAttackingState.Idle
         && owner.IsOnGround && _inventoryHandler.CanReload(weaponName);
   }
  
   public void EndReloading()
   {
      owner.onReload?.Invoke();
   }

   protected override void UpdateAnimator()
   {
      base.UpdateAnimator();
      owner.Animator.SetBool(reloadAnimParam,owner.AttackingState.CurrentState==CharacterAttackingState.Reloading);
      owner.Animator.SetFloat(reloadSpeedAnimParam,1/reloadTime);
   }
}
