using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraParams : MonoBehaviour
{
    [SerializeField]
    protected CameraParamsData data;
    [SerializeField]    
    protected CinemachineVirtualCamera vcam;
    
    protected static CameraParams instance;

    private void Awake() {
        instance = this;
        CameraParams.instance.vcam.m_Lens.OrthographicSize = CameraParams.instance.data.OrthoSizeDefault;
    }

    public static void SetOrthoSizeDefault()
    {
        CameraParams.instance.vcam.m_Lens.OrthographicSize = CameraParams.instance.data.OrthoSizeDefault;
    }
    public static void SetOrthoSizeOnDead()
    {
        CameraParams.instance.vcam.m_Lens.OrthographicSize = CameraParams.instance.data.OrthoSizeOnDead;
    }
    public static void SetOrthoSize(float size)
    {
        CameraParams.instance.vcam.m_Lens.OrthographicSize = size;
    }
    public static void SetFollowTarget(Transform tr)
    {
        CameraParams.instance.vcam.m_Follow = tr;
    }
}
