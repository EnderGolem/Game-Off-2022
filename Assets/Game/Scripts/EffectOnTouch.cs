using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class EffectOnTouch : MonoBehaviour
{
    public float DamageTakenEveryTime;

    public float DamageTakenDamageable;
    
    public float DamageTakenNonDamageable;
    
    [Tooltip("Эффекты, которые будут заложены изначально, не зависят от характеристик")]
    [SerializeField]
    protected EffectDescription[] startEffects;
    
    [Tooltip("Должен ли объект быть автоматически отключен через некоторое время после Awake")]
    [SerializeField] protected bool useAutoDisable = false;
    [SerializeField] protected float autoDisableTime;
    
    private List<PropertyManager> collidableObjects; 
    
    protected List<Effect> _effects;
    public void AddEffect(Effect ef)
    {
        _effects.Add(ef);
    }

    protected void Awake()
    {
        _effects=new List<Effect>();
        if (startEffects != null)
        {
            for (int i = 0; i < startEffects.Length; i++)
            {
                AddEffect(new Effect(startEffects[i]));
            }
            Debug.Log(gameObject.name + "effects count: "+startEffects.Length);
        }
 
        if (useAutoDisable)
        {
            Invoke("AutoDisable",autoDisableTime);
        }
    }

    protected void OnCollideWithDamageable(PropertyManager propertyManager)
    {
        for (int i = 0; i < _effects.Count; i++)
        {
            propertyManager.AddEffect(_effects[i]);
        }
    }

    protected void OnCollideWithNonDamageable(Collider2D collider)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var propertyManager = other.GetComponent<PropertyManager>();
        if (propertyManager != null)
        {
            OnCollideWithDamageable(propertyManager);
        }
        else
        {
            OnCollideWithNonDamageable(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    public void ClearEffects()
    {
        _effects.Clear();
    }
    /// <summary>
    /// Очищаем эффекты, чтобы они не стакались
    /// </summary>
    private void OnDisable()
    {
        ClearEffects();
    }

    protected void OnEnable()
    {
        _effects=new List<Effect>();
        if (startEffects != null)
        {
            for (int i = 0; i < startEffects.Length; i++)
            {
                AddEffect(new Effect(startEffects[i]));
            }
            //Debug.Log(gameObject.name + "effects count: "+startEffects.Length);
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        
        if (collider != null)
        {
            if (collider.enabled)
            {
                MMDebug.DrawGizmoCube(this.transform, 
                    new Vector3(),
                    collider.size,
                    false);
            }
            else
            {
                MMDebug.DrawGizmoCube(this.transform,
                    new Vector3(), 
                    collider.size,
                    true);
            }                
        }
    }
}
