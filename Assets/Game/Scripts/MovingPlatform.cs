using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Нужно следить, чтобы центр лифта, находился на линии двух точек, иначе он может пройти мимо.
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Точка к которой движемся")]
    public Vector2 poz1;
    [Tooltip("Точка к которой движемся")]
    public Vector2 poz2;
    [Tooltip("С какой скоростью движемся")]
    public float velocity;
    
    public bool moveToPoz1;

    protected Vector2 directionToPoz1;
    public void Awake()
    {
        directionToPoz1 = (poz1 - poz2).normalized;
    }

    public void FixedUpdate()
    {
        if( moveToPoz1 && ((Vector3)poz1 - transform.position).sqrMagnitude < velocity * velocity)
        {
            transform.position = poz1;
            moveToPoz1 = false;
            return;
        } 
        if (!moveToPoz1 &&  ((Vector3)poz2 - transform.position).sqrMagnitude < velocity * velocity)
        {
            transform.position = poz2;
            moveToPoz1 = true;
            return;
        }
        var move = directionToPoz1 * velocity;
        if (!moveToPoz1)
            move *= -1;
        transform.position += (Vector3)move;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(poz1,0.2f);
        Gizmos.DrawSphere(poz2,0.2f);
    }
}