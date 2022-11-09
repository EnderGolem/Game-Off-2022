using System;
using System.Collections;
using System.Collections.Generic;
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
    
    protected PropertyManager _propertyManager;
    protected ObjectProperty enduranceProperty;
    protected ObjectProperty enduranceRecoverSpeed;

    protected Character owner;

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
        enduranceProperty.ChangeCurValue(enduranceRecoverSpeed.GetCurValue() * Time.deltaTime);
    }

    protected void OnEnduranceChanged(float oldCurValue, float newCurValue, float oldValue, float newValue)
    {
        if (newCurValue <= 0)
        {
            owner.IsTired = true;
            return;
        }

        if (owner.IsTired && newCurValue > tirednessThreshold)
        {
            owner.IsTired = false;
        }
    }
}
