using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("Импульс придаваемый снаряду при старте по дефолту")]
    [SerializeField]
    protected float initialVelocity;
    
    protected Rigidbody2D rigidBody;

    public Rigidbody2D RigidBody => rigidBody;
    public float InitialVelocity => initialVelocity;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CalculateAngleToHitDesignatedPosition(Vector2 targetPosition,Vector2 startPosition, out float CalculatedAngle)
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        var g = Physics2D.gravity.y * rigidBody.gravityScale;

        var range = -targetPosition.x + startPosition.x;
        if (range > 0) range = -range;
        var altitude = -targetPosition.y + startPosition.y;

        var v2 = initialVelocity * initialVelocity;
        var v4 = v2 * v2;

        float angle = Mathf.Atan((v2+Mathf.Sqrt(v4-g*(g*range*range+2*altitude*v2)))/(g*range))*Mathf.Rad2Deg;
        float angle2 = Mathf.Atan((v2-Mathf.Sqrt(v4-g*(g*range*range+2*altitude*v2)))/(g*range))*Mathf.Rad2Deg;

        
        
        if (float.IsNaN(angle))
        {
            if (float.IsNaN(angle2))
            {
                CalculatedAngle = 0;
                return false;
            }

            CalculatedAngle = angle2;
            return true;
        }
        if(!float.IsNaN(angle2))
        {
            CalculatedAngle = Mathf.Min(angle, angle2);
            return true;
        }
        
        CalculatedAngle = angle;
        return true;
    }
    /// <summary>
    /// устанавливает для снаряда такой угол старта, чтобы он попал по заданным координатам
    /// </summary>
    public void SetAngleToPosition(Vector2 targetPosition)
    {
        //Debug.Log(targetPosition);
        var f = CalculateAngleToHitDesignatedPosition(targetPosition, transform.position,out float angle);
        if (f)
        {
            //Debug.Log(angle);
            if (transform.right.x >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 180-angle);
            }
        }
        
    }

    protected void StartFly()
    {
        rigidBody.velocity = transform.right * initialVelocity;
    }

    private void OnEnable()
    {
        StartFly();
    }
    
}
