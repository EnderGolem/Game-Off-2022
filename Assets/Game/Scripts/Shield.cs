using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Shield : MonoBehaviour
{
    private Character owner;

    public Collider2D OwnerCollider { get; private set; }

    public float KnockBackModifier { get; private set; }

    protected Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        Deactivate();
    }

    public void Initialize(Character owner, Collider2D ownerCollider,float knockbackModifier = 1)
    {
        this.owner = owner;
        OwnerCollider = ownerCollider;
        KnockBackModifier = knockbackModifier;
    }

    public void Activate()
    {
        collider.enabled = true;
    }

    public void Deactivate()
    {
        collider.enabled = false;
    }
}
