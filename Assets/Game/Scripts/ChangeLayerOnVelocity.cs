using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ChangeLayerOnVelocity : MonoBehaviour
{
    [SerializeField]
    private string new_layer = "Obstacles";
    [SerializeField]
    private float velocityThreshold = 1;
    [SerializeField]
    private float velocityLowerThreshold = 0.01f;
    [SerializeField]
    private bool useAngularVelocity = true;
    [SerializeField]
    private float angularVelocityThreshold=10;
    [SerializeField] 
    private float angularVelocityLowerThreshold=0.5f;
    [SerializeField]
    private bool changeChildrenLayer;

    [SerializeField] private float minTimeBetweenChanges = 1f;
    // Update is called once per frame
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    private bool hasChanged;

    private float lastChangeTime;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        lastChangeTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastChangeTime > minTimeBetweenChanges)
        {
            if (_rigidbody2D.velocity.sqrMagnitude > velocityThreshold 
                || (useAngularVelocity && _rigidbody2D.angularVelocity> angularVelocityThreshold))
            {
                _collider.usedByEffector = false;
                gameObject.layer = LayerMask.NameToLayer(new_layer);
                if (changeChildrenLayer)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(new_layer);
                    }   
                }
                hasChanged = true;
                lastChangeTime = Time.time;
            }


            if (hasChanged && _rigidbody2D.velocity.sqrMagnitude < velocityLowerThreshold 
                           && (_rigidbody2D.angularVelocity < angularVelocityLowerThreshold || !useAngularVelocity))
            {
                gameObject.layer = LayerMask.NameToLayer("BackgroundInteractables");
                if (changeChildrenLayer)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("BackgroundInteractables");
                    }   
                }
                lastChangeTime = Time.time;
            }
        }

    }
}
