using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Все способности персонажа имеют ссылку на компонент Character
/// Это сделано для того, чтобы все способности имели доступ к таким
/// общим параметрам персонажа как состояние движения и атаки
/// </summary>
public class CharacterAbility : MonoBehaviour
{
    protected Character owner;
    [Tooltip("Название конкретной способности. Используется, чтобы можно было" +
             "различать разные способности одного типа.")]
    [SerializeField]
    protected string abilityName;
    [Tooltip("Можно ли использовать способность в данный момент")]
    [SerializeField]
    public bool abilityPermitted = true;

    public string AbilityName => abilityName;

    public bool AbilityAuthorized {
        get
        {
            return abilityPermitted;
        }
    }

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
    
}
