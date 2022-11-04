using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
/// <summary>
/// Класс хранящий общие данные персонажа
/// </summary>
public class Character : MonoBehaviour, MMEventListener<MMStateChangeEvent<CharacterMovementsStates>>
{
    public MMStateMachine<CharacterMovementsStates> MovementState;
    public MMStateMachine<CharacterAttackingState> AttackingState;
    
    /// the animator associated to this character
    public Animator _animator { get; protected set; }

    public Rigidbody2D RigidBody { get; protected set; }

    public bool IsOnGround
    {
        get => Time.time - LastOnGroundTime < coyoteTime;
    }
    /// <summary>
    /// Базовая сила гравитации рассчитанная из характеристик прыжка
    /// </summary>
    public float gravityStrength { get; private set; }
    public float gravityScale { get; private set; }

    public float LastOnGroundTime { get; private set; }
    [Tooltip("Время спустя которое после физического отрыва от земли персонаж перестает считаться стоящим на земле," +
             "что позволяет ему прыгать уже сойдя с платформы")]
    [SerializeField] 
    [Range(0.01f, 0.5f)]
    protected float coyoteTime = 0.1f;
    [Header("Checks")] 
    [Tooltip("Точка в которой проходит проверка на столкновение с землей")]
    [SerializeField] private Transform _groundCheckPoint;
    [Tooltip("Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)")]
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;

    public float CoyoteTime => coyoteTime;
    private void Awake()
    {
        Debug.Log("Character");
        RigidBody = GetComponent<Rigidbody2D>();
        MovementState = new MMStateMachine<CharacterMovementsStates>(gameObject,true);
        AttackingState = new MMStateMachine<CharacterAttackingState>(gameObject, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
    }

    protected void CheckGrounded()
    {
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) /*&& !(MovementState.CurrentState == CharacterMovementsStates.Jumping)*/) //checks if set box overlaps with ground
        {
            LastOnGroundTime = Time.time; //if so sets the lastGrounded to coyoteTime
            if ((MovementState.CurrentState == CharacterMovementsStates.Jumping
                 || MovementState.CurrentState == CharacterMovementsStates.JumpFalling) && Mathf.Abs(RigidBody.velocity.y) < 0.1)
            {
                MovementState.ChangeState(CharacterMovementsStates.Idle);
            }
        }
    }

    public void CalculateGravityStrength(float jumpHeight, float jumpTimeToApex)
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
		
        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;
        
    }
    
    public void SetGravityScale(float scale)
    {
        RigidBody.gravityScale = scale;
    }

    public void OnMMEvent(MMStateChangeEvent<CharacterMovementsStates> eventType)
    {
        //Если мы только что прыгнули, то выставляем время так, чтобы не прыгнуть еще раз
        if (eventType.NewState == CharacterMovementsStates.Jumping)
        {
            LastOnGroundTime = Time.time - coyoteTime;
        }
        Debug.Log(eventType.NewState);
    }

    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }
}

public enum CharacterMovementsStates
{
    Idle, Walking, Jumping, Dashing, JumpFalling
}

public enum CharacterAttackingState
{
    Idle, Attacking
}
