using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float parallaxStrength = 0.1f;
    [SerializeField] bool disableVertivalParallax;
    Vector3 targetPreviousPosition;

    private void Start()
    {
        if (!followingTarget)
            followingTarget = Camera.main.transform;
        targetPreviousPosition = followingTarget.position;
    }

    private void Update()
    {
        var delta = followingTarget.position - targetPreviousPosition;

        if (disableVertivalParallax)
            delta.y = 0f;

        targetPreviousPosition = followingTarget.position;

        transform.Translate(delta * parallaxStrength);
    }
}
