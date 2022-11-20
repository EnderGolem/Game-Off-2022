using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementRoute : MonoBehaviour
{
    public UnityEvent OnStartRoute;
    public UnityEvent OnEndRoute;
    public enum RouteType
    {
        linear,
        loop
    }
    [Tooltip("Тип маршрута")]
    [SerializeField] RouteType routeType;
    [Tooltip("Направление движения")]
    [SerializeField] int movementDirection = 1;
    [Tooltip("Текущая точка маршрута")]
    [SerializeField] int movingTo = 0;
    [Tooltip("Список всех точек")]
    [SerializeField] List<Transform> routeElements = new List<Transform>();

    [SerializeField] RouteTracker tracker;
    [SerializeField] bool endless = false;
    [SerializeField] float dist = 0.1f;
    bool lastPoint = false;

    public void OnDrawGizmos()
    {
        if (routeElements.Count < 2) return;
        for (var i = 1; i < routeElements.Count; i++)
        {
            Gizmos.DrawLine(routeElements[i - 1].position, routeElements[i].position);
        }
        if (routeType == RouteType.loop)
        {
            Gizmos.DrawLine(First().position, Last().position);
        }
    }

    public void SetTracker(RouteTracker obj)
    {
        lastPoint=false;
        movingTo=0;
        movementDirection=1;
        if (tracker!=null)
            tracker.pointInRoute=null;

        tracker = obj;
        obj.pointInRoute = GetNextRoutePoint();
        obj.pointInRoute.MoveNext();
    }

    Transform First()
    {
        return routeElements[0];
    }

    Transform Last()
    {
        return routeElements[routeElements.Count - 1];
    }

    public IEnumerator<Transform> GetNextRoutePoint()
    {
        if (routeElements.Count < 1)
            yield break;
        while (true)
        {
            yield return routeElements[movingTo];

            if (lastPoint) yield break;
            if (routeElements.Count == 1)
                continue;
            movingTo+=movementDirection;
            if (movingTo == routeElements.Count || movingTo < 0)
            {
                if (routeType == RouteType.linear)
                {

                    if (endless)
                        movementDirection *= -1;
                    else
                        yield break;
                }
                if (routeType == RouteType.loop)
                {
                    movingTo=(routeElements.Count+movingTo)%routeElements.Count;
                    if (!endless)
                        lastPoint=true;
                }
            }
        }
    }

    [ContextMenu("Связать точки")]
    public void EditorListUpdate()
    {
        routeElements.Clear();
        var elems = GetComponentsInChildren<Transform>();
        for (var i = 0; i < elems.Length; i++)
        {
            if (elems[i] != transform)
            {
                elems[i].gameObject.name = $"Point {i}";
                routeElements.Add(elems[i]);
            }
        }
    }

    private void Update() {
        if (tracker!=null&&Vector2.Distance(tracker.transform.position,tracker.pointInRoute.Current.position)<=dist)
        {
            tracker.pointInRoute.MoveNext();
        }
    }
}
