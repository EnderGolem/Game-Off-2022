using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

//TODO Тест на 2D Тестировалось только при падении, так что при перемещении и ходьбе могут быть ошибки, но не значителеные
public class CameraTracking : MonoBehaviour
{
    public Transform trackingObject;
    [Header("Axis OX")]
    [Tooltip("На сколько умножается ускорение обьекта по оси OX")]
    public float velocityMultX = 0;
    [Tooltip("Максимальное смещение по оси OX")]
    public float maxOffsetX = 1;
    
    [Header("Axis OY")]
    [Tooltip("На сколько умножается ускорение обьекта по оси OY")]
    public float velocityMultY = 0;
    [Tooltip("Максимальное смещение по оси OY")]
    public float maxOffsetY = 1;

    protected float posZ;
    protected Vector3 centerOffset;
    protected Camera camera;
    void Start()
    {
        camera = GetComponent<Camera>();
        posZ = transform.position.z;
        transform.position = new Vector3(trackingObject.position.x, trackingObject.position.y, posZ);
        centerOffset = new Vector3();
    }

    void FixedUpdate()
    {
        float posX = trackingObject.position.x;
        float posY = trackingObject.position.y;
        if (trackingObject.GetComponent<Rigidbody>())
        {
            centerOffset.x += velocityMultX * trackingObject.GetComponent<Rigidbody>().velocity.x;
            centerOffset.x = maxOffsetY < Math.Abs(centerOffset.x)
                ? (centerOffset.x > 0 ? maxOffsetY : -maxOffsetX)
                : centerOffset.x;
            posX += centerOffset.x;
            centerOffset.y += velocityMultY * trackingObject.GetComponent<Rigidbody>().velocity.y;
            centerOffset.y = maxOffsetY < Math.Abs(centerOffset.y)
                ? (centerOffset.y > 0 ? maxOffsetY : -maxOffsetY)
                : centerOffset.y;
            posY += centerOffset.y;
        }
        transform.position = new Vector3(posX, posY, posZ);
    }

    void SetTrackingObject(Transform trackingObject)
    {
        this.trackingObject = trackingObject;
        centerOffset = Vector3.zero;
    }

    void SetZoom(float Zoom)
    {
        if(camera.orthographic)
            camera.orthographicSize = Zoom;
    }
}