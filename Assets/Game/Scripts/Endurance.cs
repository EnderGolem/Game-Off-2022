using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Endurance : PropertyObject
{
    [Tooltip("Базовая выносливость персонажа")]
    [SerializeField]
    protected float baseEndurance;
    [Tooltip("Порог выносливости при восполнении, которого персонаж выходит из состояния усталости")]
    [SerializeField]
    protected float tirednessThreshold = 25;
    [Tooltip("Количество выносливости восстанавливаемое в секунду")]
    [SerializeField]
    protected float recoverSpeed = 20;
    [Tooltip("Время через которое выносливость начинает восстанавливаться после последней траты")]
    [SerializeField]
    protected float recoverDelay;
    [Tooltip("Фидбек, вызываемый в момент когда игрок выдохся")]
    [SerializeField]
    protected MMFeedbacks fizzleOutFeedback;
    [Tooltip("фидбэк, который регулярно работает, когда персонаж устал")]
    [SerializeField]
    protected MMFeedbacks tirednessFeedback;
    
    protected ObjectProperty enduranceProperty;
    protected ObjectProperty enduranceRecoverSpeed;

    protected float lastSpentTime=-1000;

    public override void Initialize()
    {
        base.Initialize();
        enduranceRecoverSpeed = _propertyManager.AddProperty("EnduranceRecoverSpeed", recoverSpeed);
        enduranceProperty = _propertyManager.AddProperty("Endurance", baseEndurance);
        enduranceProperty.RegisterChangeCallback(OnEnduranceChanged);
    }

    private void Update()
    {
        if (Time.time - lastSpentTime > recoverDelay)
        {
            enduranceProperty.ChangeCurValue(enduranceRecoverSpeed.GetCurValue() * Time.deltaTime);
        }
        
    }

    protected void OnEnduranceChanged(float oldCurValue, float newCurValue, float oldValue, float newValue)
    {
        if (oldCurValue > newCurValue)
        {
            lastSpentTime = Time.time;
        }

        if (newCurValue <= 0)
        {
            if (!owner.IsTired)
            {
              fizzleOutFeedback?.PlayFeedbacks();   
              
              tirednessFeedback?.PlayFeedbacks();
            }
            owner.IsTired = true;
            return;
        }

        if (owner.IsTired && newCurValue > tirednessThreshold)
        {
            tirednessFeedback?.StopFeedbacks();
            
            owner.IsTired = false;
        }
    }
}
