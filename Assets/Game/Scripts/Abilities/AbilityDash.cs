using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDash : CharacterAbility<bool>
{
    [Header("Dash")]
    [Tooltip("Количество рывков, которые максимально модно накопить для совершения подряд")]
    public int dashAmount;
    [Tooltip("Скорость персонажа при рывке")]
    public float dashSpeed;
    [Tooltip("Время на которое время останавливается при использовании рывка, когда кнопка использования уже нажата, а сам рывок еще не совершен." +
             "Рекомендую устанавливать на крайне маленькое значение, чтобы замедление было почти не заметно" +
             "Но при этом игрок бы успел выбрать направление рывка")]
    public float dashSleepTime; //Duration for which the game freezes when we press dash but before we read directional input and apply a force
    [Space(5)]
    [Tooltip("Время исполнения дэша")]
    public float dashAttackTime;
    [Space(5)]
    [Tooltip("Время, между рывками из одной серии")]
    public float dashEndTime; //Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
    [Tooltip("Вектор скорости персонажа после завершения дэша относительно вектора его направления")]
    public Vector2 dashEndSpeed; //Slows down player, makes dash feel more responsive (used in Celeste)
    //[Range(0f, 1f)] public float dashEndRunLerp; //Slows the affect of player movement while dashing
    [Space(5)]
    [Tooltip("Время перезарядки заряда рывка, начиная от момента, когда персонаж касается земли")]
    public float dashRefillTime;
    [Space(5)]
    [Tooltip("Если игрок нажмет на использование рывка чуть раньше, чем это возможно, но при этом попадет в этот буфер," +
             "то рывок будет исполнен, как только это станет возможно")]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;
    [Tooltip("варианты возможных направлений для рывка")]
    public DashType dashType;
    [Tooltip("Физический слой к которому принадлежит персонаж во время рывка")]
    public string LayerWhileDashing;
    /// <summary>
    /// Последнее направление ввода
    /// </summary>
    protected Vector2 curMoveDir = Vector2.zero;
    protected Rigidbody2D rigidbody;

    protected int _dashesLeft;
    protected bool _dashRefilling;

    protected Vector2 _lastDashDir;
    
    public float LastPressedDashTime { get; private set; }
    protected override void PreInitialize()
    {
        base.PreInitialize();
        rigidbody = owner.RigidBody;
        LastPressedDashTime = -100000;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected void Update()
    {
        if (CanDash() && Time.time - LastPressedDashTime < dashInputBufferTime)
        {
            StartCoroutine(StartDash());
        }
    }

    public override void ProcessInput(bool input)
    {
        base.ProcessInput(input);
        if (input)
        {
            LastPressedDashTime = Time.time;
        }
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        curMoveDir = moveInput;
    }

    private IEnumerator StartDash()
    {
        owner.MovementState.ChangeState(CharacterMovementsStates.Dashing);
        LastPressedDashTime = Time.time - dashInputBufferTime;
        //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
        yield return owner.PerformSleep(dashSleepTime);

        //If not direction pressed, dash forward
        if (curMoveDir != Vector2.zero)
        {
            float xDir;
            switch (dashType)
            {
                //Если мы можем дэшиться в 6 разных направлений, то находим ближайшее по смыслу
                case DashType.Classic:
                    
                        xDir = 0;
                        if (curMoveDir.x != 0)
                        {
                            xDir = 0.5f * Mathf.Sign(curMoveDir.x);
                        }
                        else
                        {
                            xDir = (owner.IsFacingRight) ? 0.5f : -0.5f;
                        }

                        if (curMoveDir.y != 0)
                        {
                            _lastDashDir = new Vector2(xDir, 0.5f * Mathf.Sign(curMoveDir.y));
                        }
                        else
                        {
                            _lastDashDir = new Vector2(xDir, 0);
                        }

                        break;
                case DashType.OnlyHorizontal :
                    xDir = 0;
                    if (curMoveDir.x != 0)
                    {
                        xDir = 0.5f * Mathf.Sign(curMoveDir.x);
                    }
                    else
                    {
                        xDir = (owner.IsFacingRight) ? 0.5f : -0.5f;
                    }
                    _lastDashDir = new Vector2(xDir, 0);
                    break;
                default: _lastDashDir = curMoveDir;
                    break;
            }
            
        }
        else
        {
            _lastDashDir = owner.IsFacingRight ? Vector2.right : Vector2.left;
        }
        

        StartCoroutine(nameof(Dash), _lastDashDir);
    }

    //Dash Coroutine
    private IEnumerator Dash(Vector2 dir)
    {
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump

        float startTime = Time.time;

        _dashesLeft--;

        owner.SetGravityScale(0);

        int layer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer(LayerWhileDashing);
        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= dashAttackTime)
        {
            rigidbody.velocity = dir.normalized * dashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }

        startTime = Time.time;
        

        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        owner.SetGravityScale(owner.gravityScale);
        rigidbody.velocity = dashEndSpeed * dir.normalized;
        gameObject.layer = layer;
        while (Time.time - startTime <= dashEndTime)
        {
            yield return null;
        }
        
        //Dash over
        if (owner.IsOnGround)
        {
            owner.MovementState.ChangeState(CharacterMovementsStates.Idle);
        }
        else
        {
            owner.MovementState.ChangeState(CharacterMovementsStates.JumpFalling);
        }
    }

    //Short period before the player is able to dash again
    private IEnumerator RefillDash(int amount)
    {
        //SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
        _dashRefilling = true;
        yield return new WaitForSeconds(dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(dashAmount, _dashesLeft + 1);
    }
    
    private bool CanDash()
    {
        if (owner.MovementState.CurrentState != CharacterMovementsStates.Dashing && _dashesLeft < dashAmount && owner.IsOnGround && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return _dashesLeft > 0 && owner.MovementState.CurrentState != CharacterMovementsStates.Dashing;
    }
}

public enum DashType
{
    Classic, AnyDir, OnlyHorizontal
}
