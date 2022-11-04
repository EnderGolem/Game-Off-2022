using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

//TODO Тест на 2D Тестировалось только при падении, так что при перемещении и ходьбе могут быть ошибки, но не значителеные
public class CameraTracking : MonoBehaviour
{
    protected enum CameraEnum
    {
        Tracking,
        Moving
    }

    public Transform trackingObject;

    [Header("Axis OX")] [Tooltip("На сколько умножается ускорение обьекта по оси OX")]
    public float velocityMultX = 0;

    [Tooltip("Максимальное смещение по оси OX")]
    public float maxOffsetX = 1;

    [Header("Axis OY")] [Tooltip("На сколько умножается ускорение обьекта по оси OY")]
    public float velocityMultY = 0;

    [Tooltip("Максимальное смещение по оси OY")]
    public float maxOffsetY = 1;

    [SerializeField] protected CameraEnum cameraCondition = CameraEnum.Tracking;
    [SerializeField] protected float speedToObject;
    protected Vector3 directionToObject;
    protected float posZ;
    protected Vector3 centerOffset;
    protected Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        posZ = transform.position.z;
        transform.position = new Vector3(trackingObject.position.x, trackingObject.position.y, posZ);
        centerOffset = new Vector2();
    }

    void FixedUpdate()
    {
        switch (cameraCondition)
        {
            case CameraEnum.Tracking:
                float posX = trackingObject.position.x;
                float posY = trackingObject.position.y;
                if (trackingObject.GetComponent<Rigidbody2D>())
                {
                    centerOffset.x += velocityMultX * trackingObject.GetComponent<Rigidbody2D>().velocity.x;
                    centerOffset.x = maxOffsetY < Math.Abs(centerOffset.x)
                        ? (centerOffset.x > 0 ? maxOffsetY : -maxOffsetX)
                        : centerOffset.x;
                    posX += centerOffset.x;
                    centerOffset.y += velocityMultY * trackingObject.GetComponent<Rigidbody2D>().velocity.y;
                    centerOffset.y = maxOffsetY < Math.Abs(centerOffset.y)
                        ? (centerOffset.y > 0 ? maxOffsetY : -maxOffsetY)
                        : centerOffset.y;
                    posY += centerOffset.y;
                }
                transform.position = new Vector3(posX, posY, posZ);
                break;
            case CameraEnum.Moving: //Плавное перемещение к новому обьекту
                if (Vector3.Distance(transform.position, trackingObject.position) < speedToObject)
                {
                    cameraCondition = CameraEnum.Tracking;
                    transform.position = trackingObject.position;
                    directionToObject = Vector2.zero;
                }
                else
                    transform.position += directionToObject;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void SetTrackingObject(Transform trackingObject)
    {
        this.trackingObject = trackingObject;
        directionToObject = (trackingObject.position - transform.position).normalized;
        centerOffset = Vector2.zero;
        cameraCondition = CameraEnum.Moving;
    }

    void SetZoom(float Zoom)
    {
        if (camera.orthographic)
            camera.orthographicSize = Zoom;
    }
}