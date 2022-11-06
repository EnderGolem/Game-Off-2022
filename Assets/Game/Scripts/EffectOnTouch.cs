using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;

public class EffectOnTouch : MonoBehaviour
{
    public float DamageTakenEveryTime;

    public float DamageTakenDamageable;
    
    public float DamageTakenNonDamageable;
    
    [Tooltip("Эффекты, которые будут заложены изначально, не зависят от характеристик")]
    [SerializeField]
    protected EffectDescription[] startEffects;
    [Tooltip("Толчок придаваемый объекту при соприкосновении")]
    [SerializeField]
    protected Vector2 knockBack;
    
    [Tooltip("Должен ли объект быть автоматически отключен через некоторое время после Awake")]
    [SerializeField] protected bool useAutoDisable = false;
    [SerializeField] protected float autoDisableTime;
    [Tooltip("Следует ли отключить столкновения с владельцем этого объекта?")]
    [SerializeField] 
    protected bool ignoreOwnerCollision;
    [Tooltip("Способ определения направления толчка" +
        "OwnDir - основано на собственном направлении - подходит для зон ударов" +
         "VelocityDir - основано на направлении собственной скорости - подходит для метательных объектов")]
    [SerializeField] 
    protected KnockbackDirDefinition knockbackDirDefinition;
    
    private List<PropertyManager> collidableObjects; 
    
    protected List<Effect> _effects;
    
    protected Health objectHealth;
    protected Rigidbody2D rigidBody;
    /// <summary>
    /// Владелец объекта, наносящего урон
    /// </summary>
    protected Character owner;
    
    public void AddEffect(Effect ef)
    {
        _effects.Add(ef);
    }

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _effects=new List<Effect>();
        if (startEffects != null)
        {
            for (int i = 0; i < startEffects.Length; i++)
            {
                AddEffect(new Effect(startEffects[i]));
            }
            //Debug.Log(gameObject.name + "effects count: "+startEffects.Length);
        }
 
        if (useAutoDisable)
        {
            Invoke("AutoDisable",autoDisableTime);
        }
    }
    

    private void Start()
    {
        objectHealth = GetComponent<Health>();
    }

    protected void OnCollideWithDamageable(PropertyManager propertyManager)
    {
        
        for (int i = 0; i < _effects.Count; i++)
        {
            propertyManager.AddEffect(_effects[i]);
        }
        var body = propertyManager.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            Vector2 dir;
            if (knockbackDirDefinition == KnockbackDirDefinition.VelocityDir && rigidBody!=null)
            {
                dir = rigidBody.velocity.normalized;
            }
            else
            {
                dir = ((Vector2) transform.right).normalized;
            }
            var vec1 = dir * knockBack.x;
            Vector2 vec2;
            if (Vector2.SignedAngle(dir, Vector2.up) <= 0)
            {
                vec2 = dir.Perpendicular1() * knockBack.y;
            }
            else
            {
                vec2 = dir.Perpendicular2() * knockBack.y;
            }
            //Debug.Log($"dir = {dir}, vec1 = {vec1}, vec2 = {vec2}, sum = {vec1+vec2}");
            body.AddForce(vec1+vec2,ForceMode2D.Impulse);
        }
        objectHealth?.DoDamage(DamageTakenDamageable + DamageTakenEveryTime);
    }

    protected void OnCollideWithNonDamageable(Collider2D collider)
    {
        objectHealth?.DoDamage(DamageTakenNonDamageable + DamageTakenEveryTime);
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

    public void SetOwner(Character character)
    {
        owner = character;

        if (ignoreOwnerCollision)
        {
            Physics2D.IgnoreCollision(owner.GetComponent<Collider2D>(),GetComponent<Collider2D>());
        }
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
        //_effects=new List<Effect>();
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
/// <summary>
/// Способ определения направления толчка
/// OwnDir - основано на собственном направлении - подходит для зон ударов
/// VelocityDir - основано на направлении собственной скорости - подходит для метательных объектов
/// </summary>
public enum KnockbackDirDefinition
{
    OwnDir, VelocityDir
}
