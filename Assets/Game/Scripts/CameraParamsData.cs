using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Params", menuName = "Camera Params")]
public class CameraParamsData : ScriptableObject
{
    [Header("Стандартное отдаление камеры")]
    public float OrthoSizeDefault;
    [Header("Отдаление камеры при смерти игрока")]
    public float OrthoSizeOnDead;

}
