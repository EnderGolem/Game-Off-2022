using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Класс хранящий общие данные персонажа
/// </summary>
public class Character : MonoBehaviour, MMEventListener<MMStateChangeEvent<CharacterMovementsStates>>
{
    public MMStateMachine<CharacterMovementsStates> MovementState;
    public MMStateMachine<CharacterAttackingState> AttackingState;

    public UnityEvent<Character> OnCreate;
    public UnityEvent<Character> OnDead;
    public UnityEvent onReload;
    public Transform cameraTarget;

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public Rigidbody2D RigidBody { get; protected set; }
    private CapsuleCollider2D _Collider;

    public PropertyManager PropertyManager { get; private set; }

    public bool IsOnGround
    {
        get => Time.time - LastOnGroundTime < coyoteTime;
    }

    public bool IsOnGroundReal
    {
        get => Time.time - LastOnGroundTime < 0.001;
    }

    public bool StayOnGround { get; private set; }
    public bool StayOnPlatform { get; private set; }
    public bool StayOnStairway { get; private set; }

    public bool IsFacingRight { get; private set; } = true;

    public bool IsAlive { get; private set; } = true;

    public bool IsTired { get; set; }

    /// <summary>
    /// Базовая сила гравитации рассчитанная из характеристик прыжка
    /// </summary>
    public float gravityStrength { get; private set; }
    public float gravityScale { get; private set; }

    public float LastOnGroundTime { get; private set; }

    public bool IsPlayer => isPlayer;
    
    [SerializeField]
    protected bool isPlayer;
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
    [SerializeField] private LayerMask _groundLayer, _platformLayer, _stairwayLayer;

    [Tooltip("Список объектов, которые нужно поворачивать вместе с персонажем")]
    [SerializeField]
    protected Transform[] objectsToTurn;
    [SerializeField]
    protected Animator animator;
    [Tooltip("Фидбек, вызываемый при приземлении")]
    [SerializeField]
    protected MMFeedbacks groundedFeedback;

    public float CoyoteTime => coyoteTime;
    

    protected Vector2 curMoveInputDir;
    private void Awake()
    {
        Debug.Log("Character");
        PropertyManager = GetComponent<PropertyManager>();
        if (PropertyManager == null)//гарантия того что объекты с компонентом health имеют PropertyManager
        {
            PropertyManager = gameObject.AddComponent<PropertyManager>();
        }
        //Чтобы свойства были инициализированы раньше способностей
        var properties = GetComponents<PropertyObject>();
        foreach(var p in properties)
        {
            p.Initialize();
        }
        RigidBody = GetComponent<Rigidbody2D>();
        MovementState = new MMStateMachine<CharacterMovementsStates>(gameObject,true);
        AttackingState = new MMStateMachine<CharacterAttackingState>(gameObject, true);
        _Collider = GetComponent<CapsuleCollider2D>();
        OnCreate.Invoke(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        CheckFacing();

        StayOnStairway = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _stairwayLayer);
        StayOnPlatform = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _platformLayer);
        StayOnGround = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
        
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    //Возвращает true если коллайдер игрока все еще соприкасается с платформой
    public bool BodyInPLatform()
    {
        return Physics2D.OverlapCapsule((Vector2)transform.position+_Collider.offset, _Collider.size,_Collider.direction, 0,_platformLayer);
    }
    protected void CheckGrounded()
    {
        if (StayOnGround||StayOnPlatform||StayOnStairway) /*&& !(MovementState.CurrentState == CharacterMovementsStates.Jumping)*/ //checks if set box overlaps with ground
        {
            LastOnGroundTime = Time.time; //if so sets the lastGrounded to coyoteTime
            if ((MovementState.CurrentState == CharacterMovementsStates.Jumping
                 || MovementState.CurrentState == CharacterMovementsStates.JumpFalling) && Mathf.Abs(RigidBody.velocity.y) < 0.1)
            {
                groundedFeedback?.PlayFeedbacks();
                MovementState.ChangeState(CharacterMovementsStates.Idle);
            }
        }
    }

    protected void CheckFacing()
    {
        if (curMoveInputDir.x != 0)
        {
            if ((curMoveInputDir.x > 0 && !IsFacingRight) || (curMoveInputDir.x < 0) && IsFacingRight)
            {
                Turn();
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
    
    public void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        StartCoroutine(nameof(PerformSleep), duration);
    }

    public IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }
    
    public void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale; 
        scale.x *= -1;
        transform.localScale = scale;
        foreach (var obj in objectsToTurn)
        {
            Vector3 rot = obj.transform.localRotation.eulerAngles; 
            rot.y += 180;
            obj.transform.localRotation = Quaternion.Euler(rot);
        }
        IsFacingRight = !IsFacingRight;
    }
    
    public virtual void SetMoveInput(Vector2 input)
    {
        curMoveInputDir = input;
    }

    public void Kill()
    {
        OnDead.Invoke(this);
        GetComponentInChildren<DeadBodyRagdoll>()?.Kill();
        IsAlive = false;
    }

    public void OnMMEvent(MMStateChangeEvent<CharacterMovementsStates> eventType)
    {
        //Если мы только что прыгнули, то выставляем время так, чтобы не прыгнуть еще раз
        if (eventType.NewState == CharacterMovementsStates.Jumping)
        {
            LastOnGroundTime = Time.time - coyoteTime;
        }
        else if (eventType.NewState == CharacterMovementsStates.Dashing)
        {
            LastOnGroundTime = Time.time - coyoteTime;
        }

        //Debug.Log(eventType.NewState);
    }

    protected void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IdleMove", MovementState.CurrentState == CharacterMovementsStates.Idle);
            
        }
    }

    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }

    private void OnDrawGizmos()
    {
        MMDebug.DrawRectangle(_groundCheckPoint.position,Color.magenta, _groundCheckSize);
    }
}

public enum CharacterMovementsStates
{
    Idle, Walking, Jumping, Dashing, JumpFalling, Flying
}

public enum CharacterAttackingState
{
    Idle, Attacking, RangeAttacking,Blocking, Reloading, AttackPreparing, RangeAttackPreparing
}
