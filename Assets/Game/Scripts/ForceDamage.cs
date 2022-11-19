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
    protected Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _health = GetComponent<Health>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var h = other.collider.GetComponent<Health>();
        if (_health != null)
        {
            var diff = _rigidbody2D.velocity.magnitude*other.relativeVelocity.magnitude * _rigidbody2D.mass;

            if (diff < minDiffToDamage) return;

            Debug.Log("Force: " + damageKoef * diff + gameObject.name);

            h?.DoDamage(damageKoef * diff);
        }
    }
}
