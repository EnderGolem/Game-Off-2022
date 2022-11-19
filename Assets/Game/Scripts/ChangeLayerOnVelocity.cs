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

    [SerializeField] private float minTimeBetweenChanges = 1f;
    // Update is called once per frame
    private Rigidbody2D _rigidbody2D;

    private bool hasChanged;

    private float lastChangeTime;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        lastChangeTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastChangeTime > minTimeBetweenChanges)
        {
            if (_rigidbody2D.velocity.sqrMagnitude > velocityThreshold)
            {
                gameObject.layer = LayerMask.NameToLayer(new_layer);
                hasChanged = true;
                lastChangeTime = Time.time;
            }


            if (hasChanged && _rigidbody2D.velocity.sqrMagnitude < velocityLowerThreshold)
            {
                gameObject.layer = LayerMask.NameToLayer("BackgroundInteractables");
                lastChangeTime = Time.time;
            }
        }

    }
}
