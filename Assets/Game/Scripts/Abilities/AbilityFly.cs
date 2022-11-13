using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFly : CharacterAbility
{
    /// <summary>
    /// Последнее направление ввода
    /// </summary>
    protected Vector2 curInputDir = Vector2.zero;
    protected Rigidbody2D rigidbody;
    [Tooltip("Скорость до которой персонаж может разогнаться при беге")]
    [SerializeField]
    protected float flyMaxSpeed; //Target speed we want the player to reach.
    [Tooltip("Скорость разгона персонажа")]
    [SerializeField]
    protected float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
   
    protected float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    [Tooltip("Скорость остановки персонажа. При низком значении можно получить эффект скольжения по льду.")]
    [SerializeField]
    protected float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    protected float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player.
    [Tooltip("Гравитация персонажа во время полета")]
    [SerializeField]
    protected float flyGravityScale;
    
    protected ObjectProperty flySpeedProperty;
    
    protected override void PreInitialize()
    {
	    base.PreInitialize();
	    rigidbody = owner.RigidBody;
	    flySpeedProperty = owner.PropertyManager.AddProperty("FlySpeed", flyMaxSpeed);
    }

    private void FixedUpdate()
    {
	    if (CanFly())
	    {
		    Fly();
	    }
    }

    private void Fly()
    {
	    rigidbody.gravityScale = flyGravityScale;
        //Calculate the direction we want to move in and our desired velocity
		Vector2 targetSpeed = curInputDir * flySpeedProperty.GetCurValue();
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		//targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;
        
		runAccelAmount = (50 * runAcceleration) / flySpeedProperty.GetCurValue();
		runDeccelAmount = (50 * runDecceleration) / flySpeedProperty.GetCurValue();
		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		
		accelRate = (Mathf.Abs(targetSpeed.magnitude) > 0.01f) ? runAccelAmount : runDeccelAmount;
		#endregion

		/*#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(doConserveMomentum && Mathf.Abs(rigidbody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rigidbody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && !owner.IsOnGround)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0; 
		}
		#endregion*/

		//Calculate difference between current velocity and desired velocity
		Vector2 speedDif = targetSpeed - rigidbody.velocity;
		//Calculate force along x-axis to apply to thr player

		Vector2 movement = speedDif * accelRate;
		//Convert this to a vector and apply to rigidbody
		rigidbody.AddForce(movement, ForceMode2D.Force);

		owner.MovementState.ChangeState(CharacterMovementsStates.Flying);

    }

    protected bool CanFly()
    {
	    return AbilityAuthorized && (owner.MovementState.CurrentState == CharacterMovementsStates.Flying 
	           || owner.MovementState.CurrentState == CharacterMovementsStates.Idle);
    }

    public void ProcessInput(Vector2 input)
    {
        curInputDir = input;
    }
}
