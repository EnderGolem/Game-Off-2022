using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class SpriteTileScaler : MonoBehaviour
{
    [Tooltip("Объект с которым мы будем сравнивать размер")]
    public Transform scalableObject;
    public bool FreezeWidth;
    public bool FreezeHeight;
    public bool fixUp;
    public bool fixDown;
    private SpriteRenderer _renderer;

    private Vector3 scalableStartScale;
    private Vector3 ownStartScale;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (_renderer != null)
        {
            float x = _renderer.size.x;
            if (!FreezeWidth)
            {
                x = scalableObject.transform.localScale.x;
            }
            float y = _renderer.size.y;
            if (!FreezeHeight)
            {
                y = scalableObject.transform.localScale.y;
            }

            _renderer.size = new Vector2(x,y);
            //Debug.Log($"ownScale:{ownStartScale} scalable {scalableObject.localScale}");
            transform.localScale = new Vector2(ownStartScale.x/scalableObject.transform.localScale.x,
                ownStartScale.y/scalableObject.transform.localScale.y);
            

            if (fixUp)
            {
                transform.localPosition = new Vector2(0,((0.75f) + (scalableObject.localScale.y-1)*(0.375f))*transform.localScale.y/0.75f);
            }
            else if (fixDown)
            {
                transform.localPosition = new Vector2(0,((-0.75f) - (scalableObject.localScale.y-1)*(0.375f))*transform.localScale.y/0.75f); 
            }

            //transform.localScale = new Vector2((scalableStartScale.x / ownStartScale.x) / scalableObject.localScale.x,
            // (scalableStartScale.y / ownStartScale.y) / scalableObject.localScale.y);
        }
        else
        {
            ResetScaling();
        }
#endif
    }

    [ContextMenu("ResetScaling")]
    public void ResetScaling()
    {
        _renderer = GetComponent<SpriteRenderer>();
        ownStartScale = transform.localScale;
        scalableStartScale = scalableObject.transform.localScale;
    }
}
