using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
using UnityEngine;

public class AbilityMove : CharacterAbility
{
   /// <summary>
   /// Последнее направление ввода
   /// </summary>
   protected Vector2 curInputDir = Vector2.zero;
   protected Rigidbody2D rigidbody;
   [Tooltip("Скорость до которой персонаж может разогнаться при беге")]
   [SerializeField]
   protected float runMaxSpeed; //Target speed we want the player to reach.
   [Tooltip("Скорость разгона персонажа")]
   [SerializeField]
   protected float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
   
   protected float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
   [Tooltip("Скорость остановки персонажа. При низком значении можно получить эффект скольжения по льду.")]
   [SerializeField]
   protected float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
   protected float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
   [Space(5)]
   [Tooltip("Скорость разгона персонажа, когда он в воздухе")]
   [SerializeField]
   [Range(0f, 1)] protected float accelInAir; //Multipliers applied to acceleration rate when airborne.
   [Tooltip("Скорость остановки персонажа, когда он в воздухе")]
   [SerializeField]
   [Range(0f, 1)] protected float deccelInAir;
   [Space(5)]
   [Tooltip("Модификатор ускорения персонажа, когда он находится близко к верхней точке прыжка")]
   public float jumpHangAccelerationMult; 
   [Tooltip("Модификатор максимальной скорости персонажа, когда он находится близко к верхней точке прыжка")]
   public float jumpHangMaxSpeedMult;
   [Tooltip("Как близко к нулю должна быть скорость персонажа, чтобы считалось, что он находится в верхней точке")]
   public float jumpHangTimeThreshold;
   [Tooltip("Если игрок двигается е меняя направления выше своей максимальной скорости, то с включенным этим параметром" +
            "он скорость сбрасывать не будет")]
   public bool doConserveMomentum = true;
   [Tooltip("При каких атакующих действия следует замедлять движение")]
   [SerializeField]
   protected CharacterAttackingState[] blockAttackingStates;
   [Tooltip("На сколько уменьшается скорость движения во время выполнения атакующего действия")]
   [SerializeField]
   protected float attackingSpeedModifier = 0.4f;
   
   [Tooltip("Реальное время, на которое рассчитана анимация ходьбы")]
   [SerializeField]
   protected float actualWalkAnimationTime;
   [SerializeField]
   protected MMFeedbacks walkFeedback;

   protected ObjectProperty walkSpeedProperty;
   protected override void PreInitialize()
   {
      base.PreInitialize();
      Debug.Log("Ability");
      rigidbody = owner.RigidBody;
      walkSpeedProperty = owner.PropertyManager.AddProperty("WalkSpeed", runMaxSpeed);
   }

   protected override void Initialize()
   {
      base.Initialize();
   }

   private void FixedUpdate()
   {
      Walk();
   }

   private void Walk()
   {
      //Calculate the direction we want to move in and our desired velocity
		float targetSpeed = curInputDir.x * walkSpeedProperty.GetCurValue();

		if (blockAttackingStates.Contains(owner.AttackingState.CurrentState))
		{
			targetSpeed *= attackingSpeedModifier;
		}
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		//targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;


		runAccelAmount = (Math.Abs(runMaxSpeed) > 0.001)?(50 * runAcceleration) / runMaxSpeed:0;
		runDeccelAmount = (Math.Abs(runMaxSpeed) > 0.001)?(50 * runDecceleration) / runMaxSpeed:0;
		
	   
		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (owner.IsOnGround)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((owner.MovementState.CurrentState == CharacterMovementsStates.Jumping
			|| owner.MovementState.CurrentState == CharacterMovementsStates.JumpFalling) && Mathf.Abs(rigidbody.velocity.y) < jumpHangTimeThreshold)
		{
			accelRate *= jumpHangAccelerationMult;
			targetSpeed *= jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(doConserveMomentum && Mathf.Abs(rigidbody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rigidbody.velocity.x) == Mathf.Sign(targetSpeed) 
		   && Mathf.Abs(targetSpeed) > 0.01f && !owner.IsOnGround || owner.MovementState.CurrentState == CharacterMovementsStates.Dashing)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0; 
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - rigidbody.velocity.x;
		//Calculate force along x-axis to apply to thr player
		float movement = speedDif * accelRate;
		//Convert this to a vector and apply to rigidbody
		rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);

		if (owner.MovementState.CurrentState == CharacterMovementsStates.Idle && Mathf.Abs(targetSpeed) > 0)
		{
			owner.MovementState.ChangeState(CharacterMovementsStates.Walking);
			if (owner.IsOnGround && walkFeedback != null && !walkFeedback.IsPlaying)
			{
				walkFeedback?.PlayFeedbacks();	
			} 
		}
		
		if (owner.MovementState.CurrentState == CharacterMovementsStates.Walking && Mathf.Abs(targetSpeed) == 0)
		{
			owner.MovementState.ChangeState(CharacterMovementsStates.Idle);
			walkFeedback?.StopFeedbacks();
		}
		else if (!owner.IsOnGround)
		{
			walkFeedback?.StopFeedbacks();
		}

		/*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
   }

   protected override void UpdateAnimator()
   {
	   base.UpdateAnimator();
	   owner.Animator.SetBool("Walking", owner.MovementState.CurrentState == CharacterMovementsStates.Walking);
	   if(actualWalkAnimationTime == 0 ) Debug.LogError("Выставьте время анимации ходьбы!");
	   owner.Animator.SetFloat("WalkSpeed",(1/actualWalkAnimationTime)*(walkSpeedProperty.GetCurValue()/runMaxSpeed));
   }

   public void ProcessInput(Vector2 input)
   {
	   curInputDir = input;
   }

   protected void OnValidate()
   {

	   #region Variable Ranges
	   runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
	   runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
	   #endregion
   }
}
