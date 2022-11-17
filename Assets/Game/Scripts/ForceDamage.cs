using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public class ForceDamage : MonoBehaviour
{
    [SerializeField]
    protected float minDiffToDamage;
    [SerializeField]
    protected float damageKoef;

    protected Health _health;
    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var diff = Mathf.Abs(other.relativeVelocity.magnitude) * other.collider.attachedRigidbody.mass;

        if (diff < minDiffToDamage) return;
        
        Debug.Log("Force: "+damageKoef * diff + gameObject.name);
        
        _health?.DoDamage(damageKoef * diff);
    }
}
