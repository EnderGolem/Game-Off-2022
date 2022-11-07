using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGetDown : CharacterAbility<bool>
{
    [Header("GetDown")]
    [Space (5)]
    [Tooltip("Если игрок нажмет на спуск чуть раньше, чем достигнет платформы, то если время за которое он нажал входит во " +
             "время буфера, то спуск с платформы будет осуществлен сразу как это станет возможно")]
    [Range(0.01f, 0.5f)] public float getdownInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.
    [Tooltip("Должен ли персонаж автоматически пропускать платформы при нажатой клавише спуска")]
    [SerializeField]
    protected bool getdownAlways;
    [Tooltip("Количество выносливости, тратимой при спуске")]
    [SerializeField]
    protected float enduranceCost;
    /// <summary>
    /// Последнее направление ввода
    /// </summary>
    protected bool curInput = false;
    /// <summary>
    /// Также сохраняем ввод движения для обработки быстрого падения
    /// в случае нажатия кнопки "вниз" во время прыжка
    /// </summary>
    protected Vector2 curMoveInputDir;
    protected Rigidbody2D rigidbody;

    protected ObjectProperty enduranceProperty;
    public float LastPressedGetdownTime { get; private set; }
    protected override void PreInitialize()
    {
        base.PreInitialize();
        rigidbody = owner.RigidBody;
        enduranceProperty = owner.PropertyManager.GetPropertyByName("Endurance");
        LastPressedGetdownTime = -100000;
    }
    private bool started = false;
    private void FixedUpdate()
    { 
        ///Проверяем не нажимали ли мы спуск недавно
        if ((Time.time - LastPressedGetdownTime < getdownInputBufferTime || (getdownAlways && curInput)) && CanGetdown())
        {
            Getdown();
        }
    }

    private void Update()
    {
        if (started && !owner.BodyInPLatform())
        {
            EndOfGetDown();
            print("GET_DOWN_END!");
        }
    }

    protected void Getdown()
    {
        enduranceProperty?.ChangeCurValue(-enduranceCost);
        //Ensures we can't call GetDown multiple times from one press
        LastPressedGetdownTime = Time.time - owner.CoyoteTime;

        #region Perform Getdown
        //TODO
        started = true;
        print("GET_DOWN!");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), true);
        owner.MovementState.ChangeState(CharacterMovementsStates.Jumping);

        #endregion
    }
    protected void EndOfGetDown()
    {
        started = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
    }
    
    protected bool CanGetdown()
    {
        return !started && owner.IsOnGround && owner.MovementState.CurrentState != CharacterMovementsStates.Jumping
                                && owner.MovementState.CurrentState != CharacterMovementsStates.Dashing
            && !owner.IsTired && owner.StayOnPLatform();
    }

    public override void ProcessInput(bool input)
    {
        base.ProcessInput(input);
        if (input)
        {
            LastPressedGetdownTime = Time.time;
        }

        curInput = input;
    }
    
    public virtual void SetMoveInput(Vector2 input)
    {
        curMoveInputDir = input;
    }

    protected void OnValidate()
    {
        if (owner == null)
        {
            GetOwner();
        }
    }
}
