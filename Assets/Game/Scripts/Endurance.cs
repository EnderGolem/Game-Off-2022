using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Endurance : MonoBehaviour
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
    
    protected PropertyManager _propertyManager;
    protected ObjectProperty enduranceProperty;
    protected ObjectProperty enduranceRecoverSpeed;

    protected Character owner;

    protected float lastSpentTime=-1000;

    private void Awake()
    {
        owner = gameObject.GetComponentInParent<Character>();
        enduranceRecoverSpeed = owner.PropertyManager.AddProperty("EnduranceRecoverSpeed", recoverSpeed);
        enduranceProperty=owner.PropertyManager.AddProperty("Endurance", baseEndurance);
        enduranceProperty.RegisterChangeCallback(OnEnduranceChanged);
    }

    private void Start()
    {
        
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
            }
            owner.IsTired = true;
            return;
        }

        if (owner.IsTired && newCurValue > tirednessThreshold)
        {
            owner.IsTired = false;
        }
    }
}
