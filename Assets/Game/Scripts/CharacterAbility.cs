using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Все способности персонажа имеют ссылку на компонент Character
/// Это сделано для того, чтобы все способности имели доступ к таким
/// общим параметрам персонажа как состояние движения и атаки
/// </summary>
public class CharacterAbility<T> : MonoBehaviour
{
    protected Character owner;

    private void Awake()
    {
        GetOwner();
        PreInitialize();
    }

    private void Start()
    {
        Initialize();
    }

    protected virtual void PreInitialize()
    {
        
    }
    
    protected virtual void Initialize()
    {
        
    }

    protected void GetOwner()
    {
        owner = gameObject.GetComponentInParent<Character>();
    }

    public virtual void ProcessInput(T input)
    {
        
    }
}
