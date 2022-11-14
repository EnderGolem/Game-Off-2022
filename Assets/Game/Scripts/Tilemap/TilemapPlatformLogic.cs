using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapPlatformLogic : MonoBehaviour
{
    //Компонент PlatformEffector2D мешает работе Physics2D.IgnoreLayerCollision. Чтобы это исправить, в компоненте эффектора нужно снять флаг useColliderMask
    void Start()
    {
        PlatformEffector2D platformEffector = GetComponent<PlatformEffector2D>();
        platformEffector.useColliderMask = false;
    }
}
