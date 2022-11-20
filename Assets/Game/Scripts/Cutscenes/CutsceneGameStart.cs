using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneGameStart : MonoBehaviour
{
    public MovementRoute camRoute;
    public RouteTracker camTracker;
    public Character player;
    
    private void Start() {
        camRoute.SetTracker(camTracker);
        camTracker.moving=true;
    }
}
