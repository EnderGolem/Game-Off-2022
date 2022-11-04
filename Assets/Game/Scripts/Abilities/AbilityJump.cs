using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityJump : CharacterAbility<bool>
{
    [Header("Jump")]
    public float jumpHeight; //Height of the player's jump
    public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases thje jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangTimeThreshold; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangAccelerationMult; 
    public float jumpHangMaxSpeedMult; 	

    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.

    /// <summary>
    /// Последнее направление ввода
    /// </summary>
    protected bool curInput = false;
    protected Rigidbody2D rigidbody;
    public float LastPressedJumpTime { get; private set; }
    protected override void PreInitialize()
    {
        base.PreInitialize();
        rigidbody = owner.RigidBody;
    }

    private void FixedUpdate()
    {
        if (curInput && CanJump())
        {
            Jump();
        }
    }

    private void Update()
    {
        if (owner.MovementState.CurrentState == CharacterMovementsStates.Jumping && rigidbody.velocity.y < 0)
        {
            owner.MovementState.ChangeState(CharacterMovementsStates.Idle);
            Debug.Log("end jump = "+gameObject.transform.position.y);
        }
        
        owner.SetGravityScale(owner.gravityScale);
    }

    protected void Jump()
    {
        {
            
            owner.MovementState.ChangeState(CharacterMovementsStates.Jumping);
            //Ensures we can't call Jump multiple times from one press
            LastPressedJumpTime = Time.time + owner.CoyoteTime;

            #region Perform Jump
            //We increase the force applied if we are falling
            //This means we'll always feel like we jump the same amount 
            //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
            float force = jumpForce;
            if (rigidbody.velocity.y < 0)
                force -= rigidbody.velocity.y;
            Debug.Log("start jump = "+gameObject.transform.position.y);
            rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            #endregion
        }
    }
    
    protected bool CanJump()
    {
        return owner.IsOnGround && owner.MovementState.CurrentState != CharacterMovementsStates.Jumping
                                && owner.MovementState.CurrentState != CharacterMovementsStates.Dashing;
    }

    public override void ProcessInput(bool input)
    {
        base.ProcessInput(input);
        curInput = input;
    }

    protected void OnValidate()
    {
        if (owner == null)
        {
            GetOwner();
        }

        owner.CalculateGravityStrength(jumpHeight, jumpTimeToApex);
        
        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(owner.gravityStrength) * jumpTimeToApex;
    }
}
