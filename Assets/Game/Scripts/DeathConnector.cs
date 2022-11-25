using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Скрипт для подсоединения метательного снаряда к динамичексому объекту при попадании
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class DeathConnector : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private Health health;
    /// <summary>
    /// Был ли этот объект, когда-либо подсоединен к другому телу
    /// </summary>
    private bool wasConnected;
    /// <summary>
    /// Тело к, которому объект был подсоединен
    /// </summary>
    private Rigidbody2D connectedBody;

    private Collider2D connectedCollider;
    /// <summary>
    /// Маска коллайдеров, к которым можно прикрепиться
    /// </summary>
    public LayerMask mask;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponentInChildren<Collider2D>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (wasConnected)
        {
            if (connectedBody==null)
            {
                Destroy(gameObject);
            }
            else if(!connectedBody.gameObject.activeSelf)
            {
                rigidbody.velocity=Vector2.zero;   
            }
        }
    }
    protected void OnDeath()
    {
        List<Collider2D> colliders = new List<Collider2D>(10);
        ContactFilter2D filter= new ContactFilter2D(); 
        filter.SetLayerMask(mask);
        //filter.useLayerMask = true;
        collider.OverlapCollider(filter, colliders);
        /*Debug.Log("Death");
        foreach (var col in colliders )
        {
            Debug.Log(col.name);
        }*/
        if (colliders.Count > 0)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].GetComponent<Shield>() != null)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            
            var bodyToConnect = colliders[0].attachedRigidbody;
            
            if (bodyToConnect != null)
            {
                if (bodyToConnect.bodyType != RigidbodyType2D.Static)
                {
                    FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                    joint.connectedBody = bodyToConnect;
                    joint.enableCollision = false;
                    connectedBody = bodyToConnect;
                    connectedCollider = colliders[0];
                    wasConnected = true;
                }
                else
                {
                  rigidbody.velocity = Vector2.zero;
                  rigidbody.simulated = false;
                  
                  connectedBody = bodyToConnect;
                  connectedCollider = colliders[0];
                  wasConnected = true;
                }
            }
            var trail = GetComponentInChildren<TrailRenderer>();
            if (trail != null)
                trail.emitting = false;
        }

        collider.enabled = false;
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDeath += OnDeath;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath -= OnDeath;
        }
    }
}
